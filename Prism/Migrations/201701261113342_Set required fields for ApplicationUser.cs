namespace Prism.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetrequiredfieldsforApplicationUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Firstname", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Lastname", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Lastname", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Firstname", c => c.String());
        }
    }
}
