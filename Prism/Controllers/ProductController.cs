using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Prism.Helper;
using Prism.Models;
using Prism.DAL;
using System.Transactions;
using System.Data.Entity.Migrations;


namespace Prism.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Product/
        public ActionResult Index()
        {
            IEnumerable<Product> product;
            try
            {
                product = db.Product.Include(p => p.Company).Include(p => p.ProductVariant).Include(p => p.PriceHistory);

            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            return View(product.ToList());

        }

        // GET: /Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Include(p => p.PriceHistory).SingleOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }



        // GET: /Product/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(db.Company, "CompanyID", "Name");
            ViewBag.DepartmentID = new SelectList(db.Department, "DepartmentID", "Name");
            ViewBag.PositionID = new SelectList(db.Position, "PositionID", "positionID");
            ViewBag.ProductID = new SelectList(db.StockBalance, "ProductID", "ProductID");
            ViewBag.SectionID = new SelectList(db.Section, "SectionID", "Name");
            return View();
        }

        // POST: /Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            var applicationUser = (new SystemVariables(db)).GetCurrentUser();

            product.DateCreated = DateTime.Now;
            product.LastModified = DateTime.Now;

            //register the price in priceHistory
            var pricingHistory = new PriceHistory
            {
                ProductID = product.ProductID,
                DateInserted = DateTime.Now,
                CostPrice = product.CostPrice,
                SellingPrice = product.SellingPrice,
                SupplyPrice = product.SupplyPrice,
                ApplicationUser = applicationUser
            };

            //save all created models
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.PriceHistory.Add(pricingHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyID = new SelectList(db.Company, "CompanyID", "Name", product.CompanyID);
            ViewBag.DepartmentID = new SelectList(db.Department, "DepartmentID", "Name", product.DepartmentID);
            ViewBag.ProductID = new SelectList(db.StockBalance, "ProductID", "ProductID", product.ProductID);
            //ViewBag.SectionID = new SelectList(db.Section, "SectionID", "SectionID", product.SectionID);
            return View(product);
        }

        // GET: /Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Company, "CompanyID", "Name", product.CompanyID);
            ViewBag.DepartmentID = new SelectList(db.Department, "DepartmentID", "Name", product.DepartmentID);
            ViewBag.ProductID = new SelectList(db.StockBalance, "ProductID", "ProductID", product.ProductID);
            //ViewBag.sectionID = new SelectList(db.Section, "SectionID", "SectionID", product.SectionID);
            return View(product);
        }

        public int GetProductId(string productName)
        {
            int balance = 0;
            foreach (var variable in db.Product)
            {
                if (variable.Name == productName)
                {
                    balance = variable.ProductID;
                }
            }
            return balance;
        }

        // POST: /Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ProductID,UPC,Name,TargetGender,TargetWeather,CostPrice,SellingPrice,SupplyPrice,DepartmentID,CompanyID,PositionID")] Product product)
        {
            var p = db.Product.Find(product.ProductID);
            var dateCreated = p.DateCreated;

            if (ModelState.IsValid)
            {
                product.DateCreated = dateCreated;
                product.LastModified = DateTime.Now;

                using (var scope = new TransactionScope())
                {
                    var applicationUser = (new SystemVariables(db)).GetCurrentUser();

                    //db.Entry(product).State = EntityState.Modified;
                    db.Set<Product>().AddOrUpdate(product);

                    if (product.CostPrice != p.CostPrice || product.SellingPrice != p.SellingPrice || product.SupplyPrice != p.SupplyPrice)//check CostPrice, SellingPrice or SupplyPrice change
                    {
                        var pricingHistory = new PriceHistory
                        {
                            ProductID = product.ProductID,
                            DateInserted = DateTime.Now,
                            CostPrice = product.CostPrice,
                            SellingPrice = product.SellingPrice,
                            ApplicationUser = applicationUser
                        };
                        db.PriceHistory.Add(pricingHistory);
                    }
                    db.SaveChanges();

                    scope.Complete();

                    return RedirectToAction("Index");
                }

            }
            ViewBag.CompanyID = new SelectList(db.Company, "CompanyID", "Name", product.CompanyID);
            ViewBag.DepartmentID = new SelectList(db.Department, "DepartmentID", "Name", product.DepartmentID);
            ViewBag.ProductID = new SelectList(db.StockBalance, "ProductID", "ProductID", product.ProductID);
            return View(product);
        }

        // GET: /Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: /Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult PriceHistoryView(int id)
        {
           // PriceHistory ph = db.PriceHistory.Find(id);

            return RedirectToAction("Index", "PriceHistories", new {productId=id});



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
