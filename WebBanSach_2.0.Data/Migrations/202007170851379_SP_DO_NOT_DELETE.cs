namespace WebBanSach_2_0.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SP_DO_NOT_DELETE : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.SelectProductReport",
                body: @"SELECT a.ProductId, a.Name, c.CategoryName, b.ImportAvgPrice, b.ImportQuantity, b.ImportTotalPrice,
	                           a.Price AS ExportPrice, d.Sold AS ExportQuantity, (a.Price * d.Sold) AS ExportTotalPrice
                        FROM dbo.Products a 
                        INNER JOIN (SELECT x.ProductId, AVG(x.Price) AS ImportAvgPrice, SUM(x.AvailableQuantity) AS ImportQuantity, SUM(x.Price * x.AvailableQuantity) AS ImportTotalPrice 
			                        FROM dbo.ProductAdders x 
			                        GROUP BY x.ProductId) b
                        ON a.ProductId = b.ProductId
                        INNER JOIN dbo.ProductRanks d
                        ON d.ProductId = a.ProductId
                        INNER JOIN dbo.Categories c
                        ON c.CategoryId = a.CategoryId"
                );
            CreateStoredProcedure(
                "dbo.SelectOrder",
                p => new { OrderId = p.Int() },
                body: @"SELECT c.*, a.DiscountValue, a.DiscountCode, a.DiscountName
                        FROM dbo.Orders c
                        INNER JOIN dbo.Discounts a
                        ON c.Discount_DiscountId = a.DiscountId
                        WHERE c.OrderId = @OrderId"
                );
            CreateStoredProcedure(
                 "dbo.SelectAllProductInOrder",
                 p => new { OrderId = p.Int() },
                 body: @"SELECT a.Name as ProductName, f.Name as AuthorName, a.Price, b.Quantity, d.DiscountValue
                        FROM dbo.Products a
                        INNER JOIN dbo.OrderDetails b
                        ON b.ProductId = a.ProductId
                        INNER JOIN dbo.Orders c
                        ON b.OrderId = c.OrderId
                        INNER JOIN dbo.Discounts d
                        ON a.Discount_DiscountId = d.DiscountId
                        INNER JOIN dbo.ProductAuthors e
                        ON e.Product_ProductId = a.ProductId
                        INNER JOIN dbo.Authors f
                        ON f.AuthorId = e.Author_AuthorId
                        WHERE c.OrderId = @OrderId"
                 );
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.SelectProductReport");
            DropStoredProcedure("dbo.SelectOrder");
            DropStoredProcedure("dbo.SelectAllProductInOrder");
        }
    }
}
