namespace Prism.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCustomerIDToSupplyCart : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SupplyCarts", name: "Customer_SupplyCustomerID", newName: "SupplyCustomerID");
            RenameIndex(table: "dbo.SupplyCarts", name: "IX_Customer_SupplyCustomerID", newName: "IX_SupplyCustomerID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.SupplyCarts", name: "IX_SupplyCustomerID", newName: "IX_Customer_SupplyCustomerID");
            RenameColumn(table: "dbo.SupplyCarts", name: "SupplyCustomerID", newName: "Customer_SupplyCustomerID");
        }
    }
}
