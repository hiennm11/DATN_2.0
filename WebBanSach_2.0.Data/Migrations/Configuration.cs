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
            //var passwordHasher = new PasswordHasher();
            //var user = new ApplicationUser();
            //user.UserName = "Admin";
            //user.Email = "admin@abc.xyz";
            //user.PasswordHash = passwordHasher.HashPassword("Admin12345");
            //user.SecurityStamp = Guid.NewGuid().ToString();            

            ////Step 2 Create and add the new Role.
            //var roleToChoose = new IdentityRole("Admin");
            //context.Roles.Add(roleToChoose);            

            ////Step 3 Create a role for a user
            //var role = new IdentityUserRole();
            //role.RoleId = roleToChoose.Id;
            //role.UserId = user.Id;

            ////Step 4 Add the role row and add the user to DB)
            //user.Roles.Add(role);
            //context.Users.Add(user);

            var roleToChoose = new IdentityRole("Client");
            context.Roles.Add(roleToChoose);          
        }
    }
}
