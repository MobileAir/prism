using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Prism.DAL;
using Prism.Models;

namespace Prism.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductVariantController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductVariants
        public ActionResult Index()
        {
            var productVariant = db.ProductVariant.Include(p => p.Product);
            return View(productVariant.ToList());
        }

        // GET: ProductVariants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductVariant productVariant = db.ProductVariant.Find(id);
            if (productVariant == null)
            {
                return HttpNotFound();
            }
            return View(productVariant);
        }

        // GET: ProductVariants/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "Name");
            ViewBag.PositionID = new SelectList(db.Position, "PositionID", "PositionID");
            return View();
        }

        // POST: ProductVariants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductVariantID,Variant,UPC,PositionID,ProductID")] ProductVariant productVariant)
        {
            productVariant.DateAdded = DateTime.Now;
            productVariant.LastModified = DateTime.Now;

            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    db.ProductVariant.Add(productVariant);
                    db.SaveChanges();

                    //create entry in stockbalance
                    db.StockBalance.Add(new StockBalance() { ProductVariant = productVariant, Quantity = 0 });
                    db.SaveChanges();

                    scope.Complete();
                    
                }

                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "Name", productVariant.ProductID);
            return View(productVariant);
        }

        // GET: ProductVariants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductVariant productVariant = db.ProductVariant.Find(id);
            if (productVariant == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "Name", productVariant.ProductID);
            return View(productVariant);
        }

        // POST: ProductVariants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductVariantID,Variant,UPC,PositionID,ProductID")] ProductVariant productVariant)
        {
            var dateAdded = (db.ProductVariant.AsNoTracking().Where(v => v.ProductVariantID == productVariant.ProductVariantID)).FirstOrDefault().DateAdded;
            if (ModelState.IsValid)
            {
                productVariant.LastModified = DateTime.Now;
                productVariant.DateAdded = dateAdded;

                db.Entry(productVariant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Product, "ProductID", "Name", productVariant.ProductID);
            return View(productVariant);
        }

        // GET: ProductVariants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductVariant productVariant = db.ProductVariant.Find(id);
            if (productVariant == null)
            {
                return HttpNotFound();
            }
            return View(productVariant);
        }

        // POST: ProductVariants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductVariant productVariant = db.ProductVariant.Find(id);
            db.ProductVariant.Remove(productVariant);
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
