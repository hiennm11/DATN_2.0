namespace WebBanSach_2_0.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bigupdatedbv2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AuthorDetails", "Product_ID", "dbo.Products");
            DropIndex("dbo.AuthorDetails", new[] { "Product_ID" });
            CreateTable(
                "dbo.ProductAuthor",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        AuthorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.AuthorID })
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.AuthorDetails", t => t.AuthorID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.AuthorID);
            
            DropColumn("dbo.AuthorDetails", "Product_ID");
            DropColumn("dbo.Products", "AuthorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "AuthorID", c => c.Int(nullable: false));
            AddColumn("dbo.AuthorDetails", "Product_ID", c => c.Int());
            DropForeignKey("dbo.ProductAuthor", "AuthorID", "dbo.AuthorDetails");
            DropForeignKey("dbo.ProductAuthor", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductAuthor", new[] { "AuthorID" });
            DropIndex("dbo.ProductAuthor", new[] { "ProductID" });
            DropTable("dbo.ProductAuthor");
            CreateIndex("dbo.AuthorDetails", "Product_ID");
            AddForeignKey("dbo.AuthorDetails", "Product_ID", "dbo.Products", "ID");
        }
    }
}
