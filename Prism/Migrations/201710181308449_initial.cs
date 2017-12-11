namespace Prism.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        RecieptNumber = c.String(nullable: false),
                        NumberOfItems = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPOS = c.Boolean(nullable: false),
                        IsRolledBack = c.Boolean(nullable: false),
                        RolledBackDate = c.DateTime(),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserID = c.String(nullable: false, maxLength: 128),
                        Customer_CustomerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CartID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.Customer_CustomerID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.Customer_CustomerID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        ExpenseID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Purpose = c.String(nullable: false),
                        Recipient = c.String(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ExpenseID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PriceHistories",
                c => new
                    {
                        PriceHistoryID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        DateInserted = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CostPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SellingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SupplyPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PriceHistoryID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        TargetGender = c.Int(nullable: false),
                        TargetWeather = c.Int(nullable: false),
                        CostPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SellingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SupplyPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TargetAge = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        DepartmentID = c.Int(nullable: false),
                        CompanyID = c.Int(nullable: false),
                        Position_PositionID = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Companies", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .ForeignKey("dbo.Positions", t => t.Position_PositionID)
                .Index(t => t.Name, unique: true, name: "Name")
                .Index(t => t.DepartmentID)
                .Index(t => t.CompanyID)
                .Index(t => t.Position_PositionID);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.CompanyID)
                .Index(t => t.Name, unique: true, name: "xi_Name");
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                        SectionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentID)
                .ForeignKey("dbo.Sections", t => t.SectionID, cascadeDelete: true)
                .Index(t => t.Name, unique: true, name: "xi_Name")
                .Index(t => t.SectionID);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        SectionID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.SectionID)
                .Index(t => t.Name, unique: true, name: "xi_Name");
            
            CreateTable(
                "dbo.ProductVariants",
                c => new
                    {
                        ProductVariantID = c.Int(nullable: false, identity: true),
                        Variant = c.String(maxLength: 100),
                        UPC = c.String(nullable: false, maxLength: 20),
                        DateAdded = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        PositionID = c.String(),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductVariantID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.UPC, unique: true, name: "UPC")
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.StockBalances",
                c => new
                    {
                        ProductVariantID = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ProductVariantID)
                .ForeignKey("dbo.ProductVariants", t => t.ProductVariantID)
                .Index(t => t.ProductVariantID);
            
            CreateTable(
                "dbo.StockInItems",
                c => new
                    {
                        StockInItemID = c.Int(nullable: false, identity: true),
                        StockInID = c.Int(nullable: false),
                        ProductVariantID = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreviousQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NewBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.StockInItemID)
                .ForeignKey("dbo.ProductVariants", t => t.ProductVariantID, cascadeDelete: true)
                .ForeignKey("dbo.StockIns", t => t.StockInID, cascadeDelete: true)
                .Index(t => t.StockInID)
                .Index(t => t.ProductVariantID);
            
            CreateTable(
                "dbo.StockIns",
                c => new
                    {
                        StockInID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        InvoiceNumber = c.String(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.StockInID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SupplyCarts",
                c => new
                    {
                        SupplyCartID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        RecieptNumber = c.String(),
                        NumberOfItems = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserID = c.String(nullable: false, maxLength: 128),
                        Customer_SupplyCustomerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SupplyCartID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.SupplyCustomers", t => t.Customer_SupplyCustomerID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.Customer_SupplyCustomerID);
            
            CreateTable(
                "dbo.SupplyCustomers",
                c => new
                    {
                        SupplyCustomerID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        PhoneNumber = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.SupplyCustomerID)
                .Index(t => t.Name, unique: true, name: "Name");
            
            CreateTable(
                "dbo.SupplyPayments",
                c => new
                    {
                        SupplyPaymentID = c.Int(nullable: false, identity: true),
                        SupplyCustomerID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Remark = c.String(),
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.SupplyPaymentID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.SupplyCustomers", t => t.SupplyCustomerID, cascadeDelete: true)
                .Index(t => t.SupplyCustomerID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.SupplyCartItems",
                c => new
                    {
                        SupplyCartItemID = c.Int(nullable: false, identity: true),
                        SupplyCartID = c.Int(nullable: false),
                        ProductVariantID = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CostPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreviousStockBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SupplyCartItemID)
                .ForeignKey("dbo.ProductVariants", t => t.ProductVariantID, cascadeDelete: true)
                .ForeignKey("dbo.SupplyCarts", t => t.SupplyCartID, cascadeDelete: true)
                .Index(t => t.SupplyCartID)
                .Index(t => t.ProductVariantID);
            
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        CartItemID = c.Int(nullable: false, identity: true),
                        CartID = c.Int(nullable: false),
                        ProductVariantID = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CostPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PreviousStockBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CartItemID)
                .ForeignKey("dbo.Carts", t => t.CartID, cascadeDelete: true)
                .ForeignKey("dbo.ProductVariants", t => t.ProductVariantID, cascadeDelete: true)
                .Index(t => t.CartID)
                .Index(t => t.ProductVariantID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        SerialCode = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        PhoneNumber = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CustomerID)
                .Index(t => t.Name, unique: true, name: "Name");
            
            CreateTable(
                "dbo.POSDetails",
                c => new
                    {
                        CartID = c.Int(nullable: false),
                        POScode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CartID)
                .ForeignKey("dbo.Carts", t => t.CartID)
                .Index(t => t.CartID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        PositionID = c.String(nullable: false, maxLength: 25),
                        ShelfType = c.Int(nullable: false),
                        SerialNumber = c.Int(nullable: false),
                        Side = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PositionID)
                .Index(t => t.PositionID, unique: true, name: "Code");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Products", "Position_PositionID", "dbo.Positions");
            DropForeignKey("dbo.POSDetails", "CartID", "dbo.Carts");
            DropForeignKey("dbo.Carts", "Customer_CustomerID", "dbo.Customers");
            DropForeignKey("dbo.CartItems", "ProductVariantID", "dbo.ProductVariants");
            DropForeignKey("dbo.CartItems", "CartID", "dbo.Carts");
            DropForeignKey("dbo.Carts", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.SupplyCartItems", "SupplyCartID", "dbo.SupplyCarts");
            DropForeignKey("dbo.SupplyCartItems", "ProductVariantID", "dbo.ProductVariants");
            DropForeignKey("dbo.SupplyCarts", "Customer_SupplyCustomerID", "dbo.SupplyCustomers");
            DropForeignKey("dbo.SupplyPayments", "SupplyCustomerID", "dbo.SupplyCustomers");
            DropForeignKey("dbo.SupplyPayments", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.SupplyCarts", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PriceHistories", "ProductID", "dbo.Products");
            DropForeignKey("dbo.StockInItems", "StockInID", "dbo.StockIns");
            DropForeignKey("dbo.StockIns", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StockInItems", "ProductVariantID", "dbo.ProductVariants");
            DropForeignKey("dbo.StockBalances", "ProductVariantID", "dbo.ProductVariants");
            DropForeignKey("dbo.ProductVariants", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Departments", "SectionID", "dbo.Sections");
            DropForeignKey("dbo.Products", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.Products", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.PriceHistories", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Expenses", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Positions", "Code");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.POSDetails", new[] { "CartID" });
            DropIndex("dbo.Customers", "Name");
            DropIndex("dbo.CartItems", new[] { "ProductVariantID" });
            DropIndex("dbo.CartItems", new[] { "CartID" });
            DropIndex("dbo.SupplyCartItems", new[] { "ProductVariantID" });
            DropIndex("dbo.SupplyCartItems", new[] { "SupplyCartID" });
            DropIndex("dbo.SupplyPayments", new[] { "UserID" });
            DropIndex("dbo.SupplyPayments", new[] { "SupplyCustomerID" });
            DropIndex("dbo.SupplyCustomers", "Name");
            DropIndex("dbo.SupplyCarts", new[] { "Customer_SupplyCustomerID" });
            DropIndex("dbo.SupplyCarts", new[] { "UserID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.StockIns", new[] { "UserID" });
            DropIndex("dbo.StockInItems", new[] { "ProductVariantID" });
            DropIndex("dbo.StockInItems", new[] { "StockInID" });
            DropIndex("dbo.StockBalances", new[] { "ProductVariantID" });
            DropIndex("dbo.ProductVariants", new[] { "ProductID" });
            DropIndex("dbo.ProductVariants", "UPC");
            DropIndex("dbo.Sections", "xi_Name");
            DropIndex("dbo.Departments", new[] { "SectionID" });
            DropIndex("dbo.Departments", "xi_Name");
            DropIndex("dbo.Companies", "xi_Name");
            DropIndex("dbo.Products", new[] { "Position_PositionID" });
            DropIndex("dbo.Products", new[] { "CompanyID" });
            DropIndex("dbo.Products", new[] { "DepartmentID" });
            DropIndex("dbo.Products", "Name");
            DropIndex("dbo.PriceHistories", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.PriceHistories", new[] { "ProductID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Expenses", new[] { "UserID" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Carts", new[] { "Customer_CustomerID" });
            DropIndex("dbo.Carts", new[] { "UserID" });
            DropTable("dbo.Positions");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.POSDetails");
            DropTable("dbo.Customers");
            DropTable("dbo.CartItems");
            DropTable("dbo.SupplyCartItems");
            DropTable("dbo.SupplyPayments");
            DropTable("dbo.SupplyCustomers");
            DropTable("dbo.SupplyCarts");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.StockIns");
            DropTable("dbo.StockInItems");
            DropTable("dbo.StockBalances");
            DropTable("dbo.ProductVariants");
            DropTable("dbo.Sections");
            DropTable("dbo.Departments");
            DropTable("dbo.Companies");
            DropTable("dbo.Products");
            DropTable("dbo.PriceHistories");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Expenses");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Carts");
        }
    }
}
