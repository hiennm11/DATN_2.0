using System.Web.Mvc;

namespace WebBanSach_2_0.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "user",
                "admin/tai-khoan/user",
                new { controller = "ManageUser", action = "Index" }
            );
            context.MapRoute(
                "hoadon",
                "admin/hoa-don",
                new { controller = "ManageOrder", action = "Index" }
            );
            context.MapRoute(
                 "roles",
                 "admin/tai-khoan/roles",
                 new { controller = "ManageRole", action = "Index" }
             );
            context.MapRoute(
                "author",
                "admin/quan-ly/chi-tiet-tac-gia",
                new { controller = "Author", action = "Index" }
            );
            //context.MapRoute(
            //    "proauthor",
            //    "admin/quan-ly/tac-gia",
            //    new { controller = "ProductAuthor", action = "Index" }
            //);
            context.MapRoute(
                "theloai",
                "admin/quan-ly/the-loai",
                new { controller = "Category", action = "Index" }
            );
            context.MapRoute(
                "Product",
                "admin/quan-ly/san-pham",
                new { controller = "Product", action = "Index" }
            );

            context.MapRoute(
                "Admin_default",
                "admin/",
                new { controller = "Dashboard", action = "Index" }
            );
            context.MapRoute(
                "Func_default",
                "admin/{controller}/{action}",
                new { action = "Index"}
            );
        }
    }
}