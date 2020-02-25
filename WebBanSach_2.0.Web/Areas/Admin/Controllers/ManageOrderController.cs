using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ManageOrderController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork(new WebBanSach_2_0DbContext());
        // GET: Admin/ManageOrder
        public ActionResult Index(int page = 1)
        {
            var temp = _unitOfWork.OrderRepository.GetAll().Where(m => m.Status == true);
            var data = AutoMapperConfiguration.map.Map<IEnumerable<Order>, IEnumerable<OrderVM>>(temp);
            var pager = new Pager(data.Count(), page);

            var viewModel = new IndexViewModel<OrderVM>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.OrderModel = viewModel;
            return View();
        }
    }
}