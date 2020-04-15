using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Web.Infrastructure;
using static WebBanSach_2_0.Web.Infrastructure.Pagination;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DashboardController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            var temp = _unitOfWork.OrderRepository.GetByDateDecending();
            var data = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderVM>>(temp);
           
            ViewBag.OrderModel = data;

            return View();
        }
    }
}