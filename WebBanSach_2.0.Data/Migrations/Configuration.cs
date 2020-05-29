namespace WebBanSach_2_0.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebBanSach_2_0.Model.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<WebBanSach_2_0.Data.WebBanSach_2_0DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebBanSach_2_0.Data.WebBanSach_2_0DbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            ////Step 1 Create the user.
            var passwordHasher = new PasswordHasher();
            var user = new ApplicationUser();
            user.UserName = "Admin";
            user.Email = "admin@abc.xyz";
            user.PasswordHash = passwordHasher.HashPassword("minhhien123x");
            user.SecurityStamp = Guid.NewGuid().ToString();

            ////Step 2 Create and add the new Role.            
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

            context.Roles.Add(adRole);
            context.Roles.Add(modRole);
            context.Roles.Add(empRole);
            context.Roles.Add(userRole);

            ////Step 3 Create a role for a user
            var role = new IdentityUserRole();
            role.RoleId = adRole.Id;
            role.UserId = user.Id;

            ////Step 4 Add the role row and add the user to DB)
            user.Roles.Add(role);
            context.Users.Add(user);

            var undefinedCategory = new Category
            {
                CategoryName = "Undefined",
                CategoryId = 1
                ,
                NameAlias = "undefined",
                Status = true,
                UniqueStringKey = Guid.NewGuid()
                ,
                Description = "undefined",
                CreateBy = "admin",
                CreateDate = DateTime.Now
                ,
                UpdateBy = "admin",
                UpdatedDate = DateTime.Now
            };

            context.Categories.Add(undefinedCategory);

            var undefinedAuthor = new Author
            {
                AuthorId = 1,
                Name = "Undefined",
                NameAlias = "undefined"
                ,
                Status = true,
                UniqueStringKey = Guid.NewGuid(),
                Description = "undefined",
                CreateBy = "admin",
                CreateDate = DateTime.Now
                ,
                UpdateBy = "admin",
                UpdatedDate = DateTime.Now
            };

            context.Authors.Add(undefinedAuthor);
        }
    }
}
