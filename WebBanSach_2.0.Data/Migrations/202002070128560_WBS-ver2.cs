namespace WebBanSach_2_0.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WBSver2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "AuthorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "AuthorID", c => c.Int(nullable: false));
        }
    }
}
