using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Prism.DAL;
using Prism.Helper;
using Prism.Models;
using Prism.ViewModels;

namespace Prism.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class StockInController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StockIn
        public ActionResult Index(int? id)
        {
            var viewModel = new StockInIndexData();
            viewModel.StockIns = db.StockIn
                .Include(s => s.StockInItems.Select(si => si.ProductVariant.Product))
                .OrderByDescending(s => s.Date);

            if (id != null)
            {
                ViewBag.StockInID = id.Value;
                viewModel.StockInItems = viewModel.StockIns.Where(s => s.StockInID == id.Value).Single().StockInItems;
            }

            return View(viewModel);
        }

        // GET: StockIn/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockIn stockIn = db.StockIn.Find(id);
            if (stockIn == null)
            {
                return HttpNotFound();
            }
            return View(stockIn);
        }

        // GET: StockIn/Create
        public ActionResult Create()
        {
            var productVariantList = db.ProductVariant.ToList();
            ViewBag.ProductVariantID = new SelectList(productVariantList, "ProductVariantID", "FullName");
            return View();
        }

        // POST: StockIn/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public string Create(string stockInItemsJson, string invoiceNumber)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var stockIn = new StockIn()
                    {
                        Date = DateTime.Now,
                        ApplicationUser = (new SystemVariables(db)).GetCurrentUser(),
                        InvoiceNumber = invoiceNumber
                    };
                    db.StockIn.Add(stockIn);
                    db.SaveChanges();

                    var stockInItems = JsonConvert.DeserializeObject<ICollection<StockInItem>>(stockInItemsJson);

                    SaveStockInItems(stockInItems, stockIn.StockInID);//insert FK ID

                    UpdateStockBalance(stockInItems);

                    scope.Complete();
                    return "Stock In Successful!";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }

        }

        private void UpdateStockBalance(IEnumerable<StockInItem> stockInItems)
        {
            foreach (var stockInItem in stockInItems)
            {
                var stockBalance = db.StockBalance.Find(stockInItem.ProductVariantID);
                stockBalance.Quantity = stockBalance.Quantity + stockInItem.Quantity;

                db.Entry(stockBalance).State = EntityState.Modified;
            }
            db.SaveChanges();
        }

        private void SaveStockInItems(IEnumerable<StockInItem> stockInItems, int stockInId)
        {
            foreach (var stockInItem in stockInItems)
            {
                stockInItem.StockInID = stockInId; //insert FK ID
                stockInItem.PreviousQuantity = db.StockBalance.Find(stockInItem.ProductVariantID).Quantity;
                stockInItem.NewBalance = stockInItem.PreviousQuantity + stockInItem.Quantity;
                db.StockInItem.Add(stockInItem);
            }
            db.SaveChanges();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public decimal GetProductBalance(int ProductID)
        {
            return db.StockBalance.Find(ProductID).Quantity;
        }
    }
}
