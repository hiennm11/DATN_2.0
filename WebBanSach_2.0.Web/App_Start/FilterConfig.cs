using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Web.Infrastructure;

namespace WebBanSach_2._0.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomErrorHandler());
        }
    }
}
