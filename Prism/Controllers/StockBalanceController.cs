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

namespace Prism.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class StockBalanceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StockBalances
        public ActionResult Index()
        {
            var stockBalance = db.StockBalance
                .Include(s => s.ProductVariant.Product)
                .OrderBy(s => s.Quantity);
            return View(stockBalance.ToList());
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
