namespace WebBanSach_2_0.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NameAlias = c.String(),
                        Description = c.String(),
                        UniqueStringKey = c.Guid(nullable: false, identity: true),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorId)
                .Index(t => t.UniqueStringKey, unique: true);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Description = c.String(),
                        Image = c.String(),
                        Price = c.Double(nullable: false),
                        AvailableQuantity = c.Int(nullable: false),
                        NameAlias = c.String(),
                        Link = c.String(),
                        PublicationDate = c.DateTime(nullable: false),
                        UniqueStringKey = c.Guid(nullable: false, identity: true),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        Status = c.Boolean(nullable: false),
                        Discount_DiscountId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Discounts", t => t.Discount_DiscountId)
                .Index(t => t.CategoryId)
                .Index(t => t.UniqueStringKey, unique: true)
                .Index(t => t.Discount_DiscountId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                        Description = c.String(),
                        NameAlias = c.String(),
                        ParentId = c.Int(),
                        UniqueStringKey = c.Guid(nullable: false, identity: true),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId)
                .Index(t => t.UniqueStringKey, unique: true);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Review = c.String(),
                        Rating = c.Double(nullable: false),
                        CommentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ProductId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Address = c.String(),
                        Dob = c.DateTime(),
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
                "dbo.Discounts",
                c => new
                    {
                        DiscountId = c.Int(nullable: false, identity: true),
                        DiscountName = c.String(),
                        DiscountCode = c.String(),
                        DiscountValue = c.Double(nullable: false),
                        DiscountType = c.Int(nullable: false),
                        DiscountCover = c.String(),
                        DiscountNameAlias = c.String(),
                    })
                .PrimaryKey(t => t.DiscountId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false),
                        CustomerAddress = c.String(nullable: false),
                        CustomerEmail = c.String(nullable: false),
                        CustomerMobile = c.String(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        PaymentMethod = c.Int(nullable: false),
                        PaymentStatus = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        Discount_DiscountId = c.Int(),
                        Shipper_ShipperId = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Discounts", t => t.Discount_DiscountId)
                .ForeignKey("dbo.Shippers", t => t.Shipper_ShipperId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Discount_DiscountId)
                .Index(t => t.Shipper_ShipperId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Shippers",
                c => new
                    {
                        ShipperId = c.Int(nullable: false, identity: true),
                        ShipperName = c.String(),
                        ShipperMobile = c.String(),
                    })
                .PrimaryKey(t => t.ShipperId);
            
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
                "dbo.OrderDetails",
                c => new
                    {
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderId, t.ProductId })
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductAdders",
                c => new
                    {
                        AdderId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Name = c.String(),
                        CategoryId = c.Int(nullable: false),
                        AvailableQuantity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AdderId);
            
            CreateTable(
                "dbo.ProductRanks",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Sold = c.Int(nullable: false),
                        Rate = c.Double(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ProductAuthors",
                c => new
                    {
                        Product_ProductId = c.Int(nullable: false),
                        Author_AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_ProductId, t.Author_AuthorId })
                .ForeignKey("dbo.Products", t => t.Product_ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Authors", t => t.Author_AuthorId, cascadeDelete: true)
                .Index(t => t.Product_ProductId)
                .Index(t => t.Author_AuthorId);
            
            CreateTable(
                "dbo.DiscountApplicationUsers",
                c => new
                    {
                        Discount_DiscountId = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Discount_DiscountId, t.ApplicationUser_Id })
                .ForeignKey("dbo.Discounts", t => t.Discount_DiscountId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Discount_DiscountId)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DiscountApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DiscountApplicationUsers", "Discount_DiscountId", "dbo.Discounts");
            DropForeignKey("dbo.Products", "Discount_DiscountId", "dbo.Discounts");
            DropForeignKey("dbo.Orders", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "Shipper_ShipperId", "dbo.Shippers");
            DropForeignKey("dbo.Orders", "Discount_DiscountId", "dbo.Discounts");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.ProductAuthors", "Author_AuthorId", "dbo.Authors");
            DropForeignKey("dbo.ProductAuthors", "Product_ProductId", "dbo.Products");
            DropIndex("dbo.DiscountApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.DiscountApplicationUsers", new[] { "Discount_DiscountId" });
            DropIndex("dbo.ProductAuthors", new[] { "Author_AuthorId" });
            DropIndex("dbo.ProductAuthors", new[] { "Product_ProductId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Orders", new[] { "User_Id" });
            DropIndex("dbo.Orders", new[] { "Shipper_ShipperId" });
            DropIndex("dbo.Orders", new[] { "Discount_DiscountId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "ProductId" });
            DropIndex("dbo.Categories", new[] { "UniqueStringKey" });
            DropIndex("dbo.Products", new[] { "Discount_DiscountId" });
            DropIndex("dbo.Products", new[] { "UniqueStringKey" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.Authors", new[] { "UniqueStringKey" });
            DropTable("dbo.DiscountApplicationUsers");
            DropTable("dbo.ProductAuthors");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProductRanks");
            DropTable("dbo.ProductAdders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Shippers");
            DropTable("dbo.Orders");
            DropTable("dbo.Discounts");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.Authors");
        }
    }
}
