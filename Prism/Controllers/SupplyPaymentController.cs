using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Prism.Models;
using Prism.DAL;
using Prism.Helper;

namespace Prism.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SupplyPaymentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SupplyPayment/
        public ActionResult Index()
        {
            return View(db.SupplyPayment.ToList());
        }

        // GET: /SupplyPayment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplyPayment supplypayment = db.SupplyPayment.Find(id);
            if (supplypayment == null)
            {
                return HttpNotFound();
            }
            return View(supplypayment);
        }

        // GET: /SupplyPayment/Create
        public ActionResult Create()
        {
            ViewBag.SupplyCustomerID = new SelectList(db.SupplyCustomer, "SupplyCustomerID", "Name");
            return View();
        }

        // POST: /SupplyPayment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="SupplyCustomerID,Amount,Remark")] SupplyPayment supplyPayment)
        {
            supplyPayment.ApplicationUser = (new SystemVariables(db)).GetCurrentUser();
            supplyPayment.Date = DateTime.Now;

            var customer = db.SupplyCustomer.Find(supplyPayment.SupplyCustomerID);
            supplyPayment.SupplyCustomer = customer;
                db.SupplyPayment.Add(supplyPayment);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            
            //ViewBag.SupplyCustomerID = new SelectList(db.SupplyCustomer, "SupplyCustomerID", "Name");
            //return View(supplyPayment);
        }

        // GET: /SupplyPayment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplyPayment supplypayment = db.SupplyPayment.Find(id);
            if (supplypayment == null)
            {
                return HttpNotFound();
            }
            return View(supplypayment);
        }

        // POST: /SupplyPayment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="SupplyPaymentID,SupplyCustomerID,Amount,Date,Remark")] SupplyPayment supplypayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplypayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplypayment);
        }

        // GET: /SupplyPayment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplyPayment supplypayment = db.SupplyPayment.Find(id);
            if (supplypayment == null)
            {
                return HttpNotFound();
            }
            return View(supplypayment);
        }

        // POST: /SupplyPayment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SupplyPayment supplypayment = db.SupplyPayment.Find(id);
            db.SupplyPayment.Remove(supplypayment);
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
