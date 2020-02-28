using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;
using WebBanSach_2_0.Web.Infrastructure;
using WebBanSach_2_0.Web.Models;
using static WebBanSach_2_0.Web.Infrastructure.Pagination;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork(new WebBanSach_2_0DbContext());

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            var temp = _unitOfWork.OrderRepository.GetByDateDecending();
            var data = AutoMapperConfiguration.map.Map<IEnumerable<Order>, IEnumerable<OrderVM>>(temp);
           
            ViewBag.OrderModel = data;

            return View();
        }
    }
}