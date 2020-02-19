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
                "Admin_default",
                "admin/{controller}/{action}",
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