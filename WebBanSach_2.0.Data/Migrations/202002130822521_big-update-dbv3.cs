namespace WebBanSach_2_0.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bigupdatedbv3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductAuthor", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductAuthor", "AuthorID", "dbo.AuthorDetails");
            DropIndex("dbo.ProductAuthor", new[] { "ProductID" });
            DropIndex("dbo.ProductAuthor", new[] { "AuthorID" });
            CreateTable(
                "dbo.ProductAuthors",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        AuthorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.AuthorID })
                .ForeignKey("dbo.AuthorDetails", t => t.AuthorID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.AuthorID);
            
            DropTable("dbo.ProductAuthor");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductAuthor",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        AuthorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.AuthorID });
            
            DropForeignKey("dbo.ProductAuthors", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductAuthors", "AuthorID", "dbo.AuthorDetails");
            DropIndex("dbo.ProductAuthors", new[] { "AuthorID" });
            DropIndex("dbo.ProductAuthors", new[] { "ProductID" });
            DropTable("dbo.ProductAuthors");
            CreateIndex("dbo.ProductAuthor", "AuthorID");
            CreateIndex("dbo.ProductAuthor", "ProductID");
            AddForeignKey("dbo.ProductAuthor", "AuthorID", "dbo.AuthorDetails", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProductAuthor", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
        }
    }
}
