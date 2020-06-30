using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Service.AdminServices;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    public class ChartController : Controller
    {
        private readonly IAdminOrderService adminOrderService;

        public ChartController(IAdminOrderService adminOrderService)
        {
            this.adminOrderService = adminOrderService;
        }

        // GET: Admin/Chart
        public ActionResult Index()
        {
            
            return View();
        }
    }
}