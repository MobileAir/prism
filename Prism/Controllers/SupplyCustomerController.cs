using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Prism.DAL;
using Prism.Models;
using Prism.ViewModels;

namespace Prism.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class SupplyCustomerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SupplyCustomer
        public ActionResult Index()
        {
            return View(db.SupplyCustomer.ToList());
        }

        // GET: SupplyCustomer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplyCustomer supplyCustomer = db.SupplyCustomer.Find(id);
            if (supplyCustomer == null)
            {
                return HttpNotFound();
            }

            SupplyCustomerDetailViewModel detailedSupplyCustomer = GetDetailedSupplyCustomer(supplyCustomer);
            ViewBag.CurrentDebit = GetPreviousDebit(supplyCustomer.SupplyCustomerID);
            return View(detailedSupplyCustomer);
        }

        private SupplyCustomerDetailViewModel GetDetailedSupplyCustomer(SupplyCustomer supplyCustomer)
        {
            var detailedSupplyCustomer = new SupplyCustomerDetailViewModel();
            detailedSupplyCustomer.SupplyCustomer = supplyCustomer;

            detailedSupplyCustomer.Transactions = GetTransactions(supplyCustomer.SupplyCustomerID);

            return detailedSupplyCustomer;
        }

        private ICollection<Transaction> GetTransactions(int custID)
        {
            var debits = db.SupplyCart.Where(s => s.SupplyCustomerID == custID);
            var credits = db.SupplyPayment.Where(s => s.SupplyCustomerID == custID);

            var transactions = GetTransactions(debits, credits);
            return transactions;
        }

        private ICollection<Transaction> GetTransactions(IQueryable<SupplyCart> carts, IQueryable<SupplyPayment> payments)
        {
            var transactions = new List<Transaction>();

            foreach(var cart in carts){
                var transaction = new Transaction
                {
                    Date = cart.Date,
                    Debit = cart.TotalValue,
                    IsCredit = false,
                    Id = cart.SupplyCartID
                };
                transactions.Add(transaction);
            };

            foreach (var payment in payments)
            {
                var transaction = new Transaction
                {
                    Date = payment.Date,
                    Credit = payment.Amount,
                    IsCredit = true,
                    Remark = payment.Remark,
                    Id = payment.SupplyPaymentID
                };
                transactions.Add(transaction);
            };

            transactions = transactions.OrderBy(t => t.Date).ToList();

            return transactions;

        }

        public decimal GetPreviousDebit(int customerId)
        {
            var debit = db.SupplyCart.Where(s => s.SupplyCustomerID == customerId).Sum(s => (decimal?)(s.TotalValue)) ?? 0;
            var credit = db.SupplyPayment.Where(s => s.SupplyCustomerID == customerId).Sum(s => (decimal?)(s.Amount)) ?? 0;

            return (debit - credit);
        }

        // GET: SupplyCustomer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupplyCustomer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupplyCustomerID,Name,PhoneNumber")] SupplyCustomer supplyCustomer)
        {
            if (ModelState.IsValid)
            {
                db.SupplyCustomer.Add(supplyCustomer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplyCustomer);
        }

        // GET: SupplyCustomer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplyCustomer supplyCustomer = db.SupplyCustomer.Find(id);
            if (supplyCustomer == null)
            {
                return HttpNotFound();
            }
            return View(supplyCustomer);
        }

        // POST: SupplyCustomer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupplyCustomerID,Name,PhoneNumber")] SupplyCustomer supplyCustomer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplyCustomer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplyCustomer);
        }

        // GET: SupplyCustomer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplyCustomer supplyCustomer = db.SupplyCustomer.Find(id);
            if (supplyCustomer == null)
            {
                return HttpNotFound();
            }
            return View(supplyCustomer);
        }

        // POST: SupplyCustomer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SupplyCustomer supplyCustomer = db.SupplyCustomer.Find(id);
            db.SupplyCustomer.Remove(supplyCustomer);
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
