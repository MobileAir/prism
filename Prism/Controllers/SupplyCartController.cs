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

namespace Prism.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class SupplyCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Carts
        public ActionResult Index(DateTime? date)
        {
            if (!date.HasValue) //this is default. Which means today
            {
                date = DateTime.Now;
            }

            //get list of carts that match the date, selected user and was not rolledback
            var cartList = db.SupplyCart.Where(s => DbFunctions.TruncateTime(s.Date) == DbFunctions.TruncateTime(date)).Include(c => c.ApplicationUser).ToList().OrderByDescending(c => c.Date); //c.Date.Date wont work because DateTime.date cannot be converted to SQL 
            var users = GetUsers();
            ViewBag.UserName = new SelectList(users, "UserName", "FullName");
            ViewBag.Total = cartList.Sum(c => c.TotalValue);
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
        
        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var supplyCart = db.SupplyCart.Include(c => c.SupplyCartItems).First(c => c.SupplyCartID == id);
            if (supplyCart == null)
            {
                return HttpNotFound();
            }
            return View(supplyCart);
        }

        public ActionResult GetSupplyProductList()
        {
            var productList = from p in db.ProductVariant
                select new ProductDTO()
                {
                    UPC = p.UPC,
                    Name = p.Product.Name + " " + p.Variant, //linq to entities does not support calculated properties 'FullName'
                    UnitPrice = p.Product.SupplyPrice
                };
            
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        public decimal GetPreviousDebit(int customerId)
        {
            var debit = db.SupplyCart.Where(s => s.SupplyCustomerID == customerId).Sum(s => (decimal?)(s.TotalValue)) ?? 0;
            var credit = db.SupplyPayment.Where(s => s.SupplyCustomerID == customerId).Sum(s => (decimal?)(s.Amount)) ?? 0;

            return (debit - credit);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            ViewBag.InvoiceNumber = GenerateInvoiceNumber();
            ViewBag.SupplyCustomerID = new SelectList(db.SupplyCustomer, "SupplyCustomerID", "Name");
            return View();
        }

        private string GenerateInvoiceNumber()
        {
            string recieptNumber = ((DateTime.Now.ToLongDateString()).Replace("-", ""));
            return recieptNumber;
        }

        [HttpPost]
        public string RecordSupply(string productListJson, string recieptNumber, int customerId)
        {
            var customer = db.SupplyCustomer.Find(customerId);

            var supplyCart = new SupplyCart()
            {
                Date = DateTime.Now,
                RecieptNumber = recieptNumber,
                SupplyCustomer = customer,
                NumberOfItems = 0,
                TotalValue = 0,
                ApplicationUser = (new SystemVariables(db)).GetCurrentUser()
            };

            db.SupplyCart.Add(supplyCart);
            db.SaveChanges();

            var products = JsonConvert.DeserializeObject<ICollection<CartItemViewModel>>(productListJson); //deserialize json string to cartItem collection

            var cartItems = ConvertProductListToSupplyItems(products, supplyCart.SupplyCartID); //convert cartItems to match Sale table

            var cartItemsList = cartItems as IList<SupplyCartItem> ?? cartItems.ToList();
            foreach (var item in cartItemsList)
            {
                UpdateQuantity(item, "subtract");
                db.SupplyCartItem.Add(item);
            }
            
            UpdateCartDetails(supplyCart.SupplyCartID, cartItemsList);

            db.SaveChanges();

            return cartItemsList.Sum(item => item.Quantity).ToString(CultureInfo.InvariantCulture);
            //return "saved";
        }

        private void UpdateCartDetails(int cartId, IList<SupplyCartItem> cartItemsList)
        {
            var supplyCart = db.SupplyCart.Find(cartId);
            supplyCart.NumberOfItems = cartItemsList.Sum(item => item.Quantity);
            supplyCart.TotalValue = cartItemsList.Sum(item => item.Quantity * item.UnitPrice);
            db.Entry(supplyCart).State = EntityState.Modified;
            db.SaveChanges();
        }

        private IEnumerable<SupplyCartItem> ConvertProductListToSupplyItems(ICollection<CartItemViewModel> products, int supplyCartId)
        {
            var cartItems = new List<SupplyCartItem>();
            foreach (var product in products)
            {
                var pro = db.ProductVariant.First(p => p.UPC == product.UPC);
                var cartItem = new SupplyCartItem
                {
                    SupplyCartID = supplyCartId,
                    ProductVariantID = pro.ProductVariantID,
                    Quantity = decimal.Parse(product.Qty),
                    UnitPrice = pro.Product.SupplyPrice,
                    PreviousStockBalance = db.StockBalance.Find(pro.ProductVariantID).Quantity
                };

                cartItems.Add(cartItem);
            }

            return cartItems;
        }

        private void UpdateQuantity(SupplyCartItem item, string operand)
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
            var supplyCart = db.SupplyCart.Include(c => c.SupplyCartItems).First(c => c.SupplyCartID == id);
            if (supplyCart == null)
            {
                return HttpNotFound();
            }
            return View(supplyCart);
        }

        // POST: Carts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    var cart = db.SupplyCart.Find(id);
        //    cart.IsRolledBack = true;
        //    cart.RolledBackDate = DateTime.Now;
        //    db.Entry(cart).State = EntityState.Modified;


        //    var supplyCartItems = db.SupplyCartItem.Where(c => c.SupplyCartID == id);

        //    foreach (var supplyCartItem in supplyCartItems)
        //    {
        //        UpdateQuantity(supplyCartItem, "Add");
        //    }

        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
