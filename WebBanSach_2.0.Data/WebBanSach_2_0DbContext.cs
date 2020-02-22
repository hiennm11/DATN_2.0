using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.Models;

namespace WebBanSach_2_0.Data
{
    public class WebBanSach_2_0DbContext : IdentityDbContext<ApplicationUser>
    {
        public WebBanSach_2_0DbContext() : base("Data Source=.;Initial Catalog=WebBanSach2DB;Integrated Security=True;MultipleActiveResultSets=True")
        {
            
        }
     
        public DbSet<AuthorDetail> AuthorDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAuthor> ProductAuthors { get; set; }
        public DbSet<Error> Errors { get; set; }

        public static WebBanSach_2_0DbContext Create()
        {
            return new WebBanSach_2_0DbContext();
        }

    }
}
