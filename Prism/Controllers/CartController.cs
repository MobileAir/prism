using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web.Mvc;
using Prism.DAL;
using Prism.DTO;
using Prism.Helper;
using Prism.Models;
using Prism.ViewModels;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using System.Transactions;

namespace Prism.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Carts
        public ActionResult Index(DateTime? date, string Id)
        {
            if (!date.HasValue) //this is default. Which means today
            {
                date = DateTime.Now;
            }            

            //get list of carts that match the date, selected user and was not rolledback
            var cartList = (db.Cart.Where(c => c.IsRolledBack == false && DbFunctions.TruncateTime(c.Date) == DbFunctions.TruncateTime(date)).Include(c => c.ApplicationUser).Include(c => c.Customer).ToList().OrderByDescending(c => c.Date)).ToList(); //c.Date.Date wont work because DateTime.date cannot be converted to SQL 

            if (Id != null && Id != "")
            {
                var user = db.Users.Find(Id);
                ViewBag.User = user.Fullname;
                cartList = cartList.Where(c => c.ApplicationUser == user).ToList();
            }
            else
            {
                ViewBag.User = "All Users";
            }
            var cashCartList = cartList.Where(c => c.IsPOS == false);
            var posCartList = cartList.Where(c => c.IsPOS);
            
            var users = GetUsers();

            ViewBag.Id = new SelectList(users, "Id", "FullName");
            ViewBag.Total = cartList.Sum(c => c.TotalValue);
            ViewBag.CashTotal = cashCartList.Sum(c => c.TotalValue);
            ViewBag.POSTotal = posCartList.Sum(c => c.TotalValue);

            ViewBag.CashCount = cashCartList.Count();
            ViewBag.POSCount = posCartList.Count();

            ViewBag.Day = GetDayString(date);

            return View(cartList);
        }

        private object GetDayString(DateTime? date)
        {
            if (!date.HasValue || date.Value.Date == DateTime.Now.Date)
            {
                return "Today";
            }
            return date.Value.ToShortDateString();
        }

        private IEnumerable<EditUserViewModel> GetUsers()
        {
            var Db = new ApplicationDbContext();
            var users = Db.Users;
            var model = new List<EditUserViewModel>();
            foreach (var user in users)
            {
                var u = new EditUserViewModel(user);
                model.Add(u);
            }
            return model;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RollBack()
        {
            return View(db.Cart.Where(c => c.IsRolledBack).ToList().OrderByDescending(c => c.Date));
        }
        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Cart cart = db.Cart.Include(c => c.CartItems).First(c => c.CartID == id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.Rollback = cart.IsRolledBack ? "true" : "false"; //used to display proper labels in view
            return View(cart);
        }

        public ActionResult GetProductList()
        {
            var productList = from p in db.ProductVariant
                select new ProductDTO()
                {
                    UPC = p.UPC,
                    Name = p.Product.Name + " " + p.Variant, //linq to entities does not support calculated properties 'FullName'
                    UnitPrice = p.Product.SellingPrice
                };
            
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            ViewBag.RecieptNumber = GenerateRecieptNumber();
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "Name");
            return View();
        }

        private string GenerateRecieptNumber()
        {
            string recieptNumber = ((DateTime.Now.ToLongDateString()).Replace("-", ""));
            return recieptNumber;
        }

        [HttpPost]
        public string RecordSale(string productListJson, string recieptNumber, int customerId, string amountPaid, string balance, bool isPOS, string posCode)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var customer = db.Customer.Find(customerId);
                    var cart = new Cart()
                    {
                        Date = DateTime.Now,
                        RecieptNumber = recieptNumber,
                        Customer = customer,
                        NumberOfItems = 0,
                        TotalValue = 0,
                        IsPOS = isPOS,
                        AmountPaid = Decimal.Parse(amountPaid),
                        Balance = Decimal.Parse(balance),
                        ApplicationUser = (new SystemVariables(db)).GetCurrentUser()
                    };

                    db.Cart.Add(cart);
                    db.SaveChanges();

                    var products = JsonConvert.DeserializeObject<ICollection<CartItemViewModel>>(productListJson); //deserialize json string to cartItem collection

                    var cartItems = ConvertProductListToSaleItems(products, cart.CartID); //convert cartItems to match Sale table

                    var cartItemsList = cartItems as IList<CartItem> ?? cartItems.ToList();
                    foreach (var item in cartItemsList)
                    {
                        UpdateQuantity(item, "subtract");
                        db.CartItem.Add(item);
                    }

                    UpdateCartDetails(cart.CartID, cartItemsList);

                    if (isPOS) SavePOSDetails(cart, posCode);
                    
                    db.SaveChanges();

                    scope.Complete();

                    return cartItemsList.Sum(item => item.Quantity).ToString(CultureInfo.InvariantCulture);
                    //return "saved"                   
                    
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            
        }

        private void SavePOSDetails(Cart cart, string POSCode)
        {
            
            var posDetail = new POSDetail()
            {
                Cart = cart,
                POScode = POSCode
            };
            db.POSDetail.Add(posDetail);
        }

        private void UpdateCartDetails(int cartId, IList<CartItem> cartItemsList)
        {
            var cart = db.Cart.Find(cartId);
            cart.NumberOfItems = cartItemsList.Sum(item => item.Quantity);
            cart.TotalValue = cartItemsList.Sum(item => item.Quantity*item.UnitPrice);
            db.Entry(cart).State = EntityState.Modified;
            db.SaveChanges();
        }

        private IEnumerable<CartItem> ConvertProductListToSaleItems(ICollection<CartItemViewModel> products, int cartId)
        {
            var cartItems = new List<CartItem>();
            foreach (var product in products)
            {
                var pro = db.ProductVariant.First(p => p.UPC == product.UPC);
                var cartItem = new CartItem
                {
                    CartID = cartId,
                    ProductVariantID = pro.ProductVariantID,
                    Quantity = decimal.Parse(product.Qty),
                    UnitPrice = pro.Product.SellingPrice,
                    PreviousStockBalance = db.StockBalance.Find(pro.ProductVariantID).Quantity
                };

                cartItems.Add(cartItem);
            }

            return cartItems;
        }

        private void UpdateQuantity(CartItem item, string operand)
        {
            var stockBalance = db.StockBalance.Find(item.ProductVariantID);
            if (operand == "subtract")
            {
                stockBalance.Quantity = stockBalance.Quantity - item.Quantity;
            }
            else
            {
                stockBalance.Quantity = stockBalance.Quantity + item.Quantity;
            }
            

            db.Entry(stockBalance).State = EntityState.Modified;
        }

       // GET: Carts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Cart.Include(c => c.CartItems).First(c => c.CartID == id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Cart.Find(id);
            cart.IsRolledBack = true;
            cart.RolledBackDate = DateTime.Now;
            db.Entry(cart).State = EntityState.Modified;


            var cartItems = db.CartItem.Where(c => c.CartID == id);

            foreach (var cartItem in cartItems)
            {
                UpdateQuantity(cartItem, "Add");
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
