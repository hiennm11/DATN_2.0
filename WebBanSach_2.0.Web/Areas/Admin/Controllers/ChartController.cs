using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Service.Interfaces;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    public class ChartController : Controller
    {
        private readonly IAdminOrderService _adminOrderService;

        public ChartController(IAdminOrderService adminOrderService)
        {
            this._adminOrderService = adminOrderService;
        }

        // GET: Admin/Chart
        public async Task<ActionResult> Index()
        {
            var response = await _adminOrderService.GetChartResponse();
           
            return View(response);
        }

    }
}