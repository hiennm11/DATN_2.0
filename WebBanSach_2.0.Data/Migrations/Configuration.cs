namespace WebBanSach_2_0.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebBanSach_2_0.Model.Entities;
    using WebBanSach_2_0.Model.Enums;

    internal sealed class Configuration : DbMigrationsConfiguration<WebBanSach_2_0DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebBanSach_2_0DbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
           

            #region Required Data
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            
            var adRole = new IdentityRole()
            {
                Id = "admin",
                Name = "Admin"
            };

            var modRole = new IdentityRole()
            {
                Id = "mod",
                Name = "Quản lý"
            };

            var empRole = new IdentityRole()
            {
                Id = "employee",
                Name = "Nhân viên"
            };

            var userRole = new IdentityRole()
            {
                Id = "user",
                Name = "User"
            };

            var shipperRole = new IdentityRole()
            {
                Id = "shipper",
                Name = "Shipper"
            };

            context.Roles.Add(adRole);
            context.Roles.Add(modRole);
            context.Roles.Add(empRole);
            context.Roles.Add(userRole);
            context.Roles.Add(shipperRole);

            var userToInsert = new ApplicationUser { UserName = "admin@abc.xyz", PhoneNumber = "12345678911", Email = "admin@abc.xyz" };

            var role = new IdentityUserRole();
            role.RoleId = adRole.Id;
            role.UserId = userToInsert.Id;

            userToInsert.Roles.Add(role);
            userManager.Create(userToInsert, "admin@123");


            var undefinedCategory = new Category
            {
                CategoryName = "Undefined",
                CategoryId = 1,
                NameAlias = "undefined",
                Status = true,
                UniqueStringKey = Guid.NewGuid(),
                Description = "undefined",
                CreateBy = "admin",
                CreateDate = DateTime.Now,
                UpdateBy = "admin",
                UpdatedDate = DateTime.Now
            };

            context.Categories.Add(undefinedCategory);

            var undefinedAuthor = new Author
            {
                AuthorId = 1,
                Name = "Undefined",
                NameAlias = "undefined",
                Status = true,
                UniqueStringKey = Guid.NewGuid(),
                Description = "undefined",
                CreateBy = "admin",
                CreateDate = DateTime.Now,
                UpdateBy = "admin",
                UpdatedDate = DateTime.Now
            };

            context.Authors.Add(undefinedAuthor);

            var ZeroDiscount = new Discount
            {
                DiscountId = 1, 
                DiscountType = Model.Enums.DiscountType.All,
                DiscountName = "DefaultPrice",
                DiscountValue = 0
            };

            context.Discounts.Add(ZeroDiscount);

            #endregion

            //Seed Mock Data


        }

        private void SeedMockData(WebBanSach_2_0DbContext context, bool isTrue)
        {
            if (isTrue)
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var ZeroDiscount = context.Discounts.FirstOrDefault(m => m.DiscountId == 1);
                var undefinedAuthor = context.Authors.FirstOrDefault(m => m.AuthorId == 1);


                //Seed Mock Data
                //Add Product
                for (int i = 0; i < 30; i++)
                {
                    var product = new Product
                    {
                        ProductId = i,
                        CategoryId = 1,
                        CreateDate = DateTime.Now,
                        PublicationDate = DateTime.Now,
                        CreateBy = "admin",
                        Name = "Book " + i,
                        NameAlias = "book-" + i,
                        Price = 10000,
                        Status = true
                    };
                    product.Discount = ZeroDiscount;
                    product.Authors = new List<Author> { undefinedAuthor };
                    context.Products.Add(product);
                    context.SaveChanges();

                    var rank = new ProductRank { ProductId = product.ProductId, Name = product.Name, CategoryId = product.CategoryId, Rate = 0, Sold = 0 };
                    context.ProductRanks.Add(rank);
                }

                //Add Account
                string[] roles = new string[4] { "user", "mod", "employee", "shipper" };
                Random random = new Random();
                List<ApplicationUser> users = new List<ApplicationUser>();
                for (int i = 0; i < 100; i++)
                {
                    var clients = new ApplicationUser
                    {
                        UserName = "client" + i + "@abc.xyz",
                        PhoneNumber = "12345678911",
                        Email = "client" + i + "@abc.xyz",
                        Address = "test",
                        FullName = "client" + i,
                        Dob = DateTime.Now,
                    };

                    var clientsRole = new IdentityUserRole();
                    clientsRole.RoleId = roles[random.Next(roles.Length)];
                    clientsRole.UserId = clients.Id;

                    clients.Roles.Add(clientsRole);
                    userManager.Create(clients, "client@123");

                    if (clientsRole.RoleId == "user")
                    {
                        users.Add(clients);
                    }

                }

                //Add Order
                string[] orderroles = new string[3] { "admin", "mod", "employee" };
                var productList = context.Products.ToList();
                Random rd = new Random();
                for (int i = 0; i < 200; i++)
                {
                    var userRd = users[random.Next(users.Count())];
                    var order = new Order
                    {
                        CustomerName = userRd.FullName,
                        CustomerAddress = userRd.Address,
                        CustomerMobile = userRd.PhoneNumber,
                        CustomerEmail = userRd.Email,
                        PaymentMethod = (PaymentMethod)random.Next(4),
                        PaymentStatus = false,
                        CreatedDate = new DateTime(DateTime.Now.Year, rd.Next(3, 7), rd.Next(1, 31)),
                        CreatedBy = orderroles[random.Next(orderroles.Length)],
                        Status = (OrderStatus)random.Next(10),
                        Discount = ZeroDiscount,

                    };

                    context.Orders.Add(order);
                    context.SaveChanges();
                }

                for (int i = 1; i <= context.Orders.Count(); i++)
                {
                    context.OrderDetails.Add(new OrderDetail { OrderId = i, ProductId = productList[random.Next(productList.Count())].ProductId, Quantity = 1 });
                }

            }
        }
    }
}
