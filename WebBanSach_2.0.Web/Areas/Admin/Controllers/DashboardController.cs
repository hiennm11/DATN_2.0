using System.Threading.Tasks;
using System.Web.Mvc;
using WebBanSach_2_0.Service.AdminServices;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAdminOrderService _adminOrderService;

        public DashboardController(IAdminOrderService adminOrderService)
        {
            this._adminOrderService = adminOrderService;
        }

        // GET: Admin/Dashboard
        public async Task<ActionResult> Index()
        {
            var response = await _adminOrderService.GetDashboardResponse();

            return View(response);
        }

        public ActionResult GetOrderDetailPartial(int id)
        {
            return Task.Run(async () =>
            {
                var cart = await _adminOrderService.GetOrderDetailCartView(id);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_OrderDetail", cart);
                }
                return PartialView("_OrderDetail", cart);
            }).Result;
        }
    }
}