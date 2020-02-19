namespace WebBanSach_2_0.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bigupdatedbv1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Authors", "AuthorID", "dbo.AuthorDetails");
            DropForeignKey("dbo.Authors", "ProductID", "dbo.Products");
            DropIndex("dbo.Authors", new[] { "ProductID" });
            DropIndex("dbo.Authors", new[] { "AuthorID" });
            AddColumn("dbo.AuthorDetails", "Product_ID", c => c.Int());
            AddColumn("dbo.Products", "AuthorID", c => c.Int(nullable: false));
            CreateIndex("dbo.AuthorDetails", "Product_ID");
            AddForeignKey("dbo.AuthorDetails", "Product_ID", "dbo.Products", "ID");
            DropTable("dbo.Authors");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        AuthorID = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => new { t.ProductID, t.AuthorID });
            
            DropForeignKey("dbo.AuthorDetails", "Product_ID", "dbo.Products");
            DropIndex("dbo.AuthorDetails", new[] { "Product_ID" });
            DropColumn("dbo.Products", "AuthorID");
            DropColumn("dbo.AuthorDetails", "Product_ID");
            CreateIndex("dbo.Authors", "AuthorID");
            CreateIndex("dbo.Authors", "ProductID");
            AddForeignKey("dbo.Authors", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Authors", "AuthorID", "dbo.AuthorDetails", "ID", cascadeDelete: true);
        }
    }
}
