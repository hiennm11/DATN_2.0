using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Migrations;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data
{
    public class WebBanSach_2_0DbContext : IdentityDbContext<ApplicationUser>
    {
        public WebBanSach_2_0DbContext() : base("BookStoreDB")
        {
            ////Database.SetInitializer(new MigrateDatabaseToLatestVersion<WebBanSach_2_0DbContext, Configuration>());
        }
     
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<ProductRank> ProductRanks { get; set; }
        public DbSet<ProductAdder> ProductAdders { get; set; }

        public static WebBanSach_2_0DbContext Create()
        {
            return new WebBanSach_2_0DbContext();
        }

    }
}
