using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Prism.Models;

namespace Prism.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection",
            throwIfV1Schema: false)
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<ApplicationRole> IdentityRoles { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<PriceHistory> PriceHistory { get; set; }
        public DbSet<StockBalance> StockBalance { get; set; }
        public DbSet<StockInItem> StockInItem { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<StockIn> StockIn { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<ProductVariant> ProductVariant { get; set; }
        public DbSet<Expense> Expense { get; set; }
        public DbSet<SupplyCustomer> SupplyCustomer { get; set; }
        public DbSet<SupplyCart> SupplyCart { get; set; }
        public DbSet<SupplyCartItem> SupplyCartItem { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<POSDetail> POSDetail { get; set; }
        public DbSet<SupplyPayment> SupplyPayment { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cart>()
                .HasRequired(e => e.ApplicationUser)
                .WithMany(t => t.Carts)
                .Map(m => m.MapKey("UserID"));

            modelBuilder.Entity<Expense>()
                .HasRequired(e => e.ApplicationUser)
                .WithMany(t => t.Expenses)
                .Map(m => m.MapKey("UserID"));

            modelBuilder.Entity<StockIn>()
                .HasRequired(e => e.ApplicationUser)
                .WithMany(t => t.StockIns)
                .Map(m => m.MapKey("UserID"));

            modelBuilder.Entity<SupplyCart>()
                .HasRequired(e => e.ApplicationUser)
                .WithMany(t => t.SupplyCarts)
                .Map(m => m.MapKey("UserID"));

            modelBuilder.Entity<SupplyPayment>()
                .HasRequired(e => e.ApplicationUser)
                .WithMany(t => t.SupplyPayments)
                .Map(m => m.MapKey("UserID"));
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
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
        }

    }
}