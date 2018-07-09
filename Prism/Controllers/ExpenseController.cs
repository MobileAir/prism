using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Prism.DAL;
using Prism.Helper;
using Prism.Models;

namespace Prism.Controllers
{
    //[Authorize(Roles="Admin, Manager")]
    public class ExpenseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Expense
        public ActionResult Index(DateTime? date)
        {
            if (date == null)
            {
                ViewBag.Date = "Today";
                return View(db.Expense.Where(e => DbFunctions.TruncateTime(e.Date) == DbFunctions.TruncateTime(DateTime.UtcNow)).ToList());

            }
            else
            {
                ViewBag.Date = ((DateTime)date).ToShortDateString();
                return View(db.Expense.Where(e => DbFunctions.TruncateTime(e.Date) == DbFunctions.TruncateTime(date)).ToList());
            }

        }

        // GET: Expense/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expense.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // GET: Expense/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Expense/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Expense expense)
        {
            var userID = User.Identity.GetUserId();
            var currentUser = (new SystemVariables(db)).GetCurrentUser();

            expense.Date = DateTime.Now;
            expense.ApplicationUser = currentUser;


                db.Expense.Add(expense);
                db.SaveChanges();
                return RedirectToAction("Index");


        }

        // GET: Expense/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expense.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // POST: Expense/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExpenseID,Date,Amount,Purpose,Recipient")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expense);
        }

        // GET: Expense/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expense.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // POST: Expense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expense expense = db.Expense.Find(id);
            db.Expense.Remove(expense);
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
