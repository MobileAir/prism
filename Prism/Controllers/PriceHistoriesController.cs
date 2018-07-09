using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Prism.DAL;
using Prism.Helper;
using Prism.Models;

namespace Prism.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class PriceHistoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PriceHistories
        public ActionResult Index(int productId)
        {
            var movies = from m in db.PriceHistory
                          select m;

            //if (!String.IsNullOrEmpty(productId))
            if (productId !=0)
            {
                movies = movies.Where  ( s => s.ProductID.Equals(productId));
            }

            return View(movies.ToList());
            //var priceHistory = db.PriceHistory.Include(p => p.Product);
            //return View(priceHistory.ToList());
        }

        // GET: PriceHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceHistory priceHistory = db.PriceHistory.Find(id);
            if (priceHistory == null)
            {
                return HttpNotFound();
            }
            return View(priceHistory);
        }

        // GET: PriceHistories/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "UPC");
            return View();
        }

        // POST: PriceHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PriceHistoryID,ProductID,DateInserted,CostPrice,SellingPrice")] PriceHistory priceHistory)
        {
            if (ModelState.IsValid)
            {
                priceHistory.ApplicationUser = (new SystemVariables(db)).GetCurrentUser();
                db.PriceHistory.Add(priceHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "UPC", priceHistory.ProductID);
            return View(priceHistory);
        }

        // GET: PriceHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceHistory priceHistory = db.PriceHistory.Find(id);
            if (priceHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "UPC", priceHistory.ProductID);
            return View(priceHistory);
        }

        // POST: PriceHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PriceHistoryID,ProductID,DateInserted,CostPrice,SellingPrice")] PriceHistory priceHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(priceHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "UPC", priceHistory.ProductID);
            return View(priceHistory);
        }

        // GET: PriceHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceHistory priceHistory = db.PriceHistory.Find(id);
            if (priceHistory == null)
            {
                return HttpNotFound();
            }
            return View(priceHistory);
        }

        // POST: PriceHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PriceHistory priceHistory = db.PriceHistory.Find(id);
            db.PriceHistory.Remove(priceHistory);
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
