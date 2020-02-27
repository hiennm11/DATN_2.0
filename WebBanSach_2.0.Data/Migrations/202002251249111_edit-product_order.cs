namespace WebBanSach_2_0.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editproduct_order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Link", c => c.String());
            AlterColumn("dbo.Orders", "PaymentStatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Orders", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Orders", "PaymentStatus", c => c.String());
            DropColumn("dbo.Products", "Link");
        }
    }
}
