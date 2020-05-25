using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ViewModels;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public DashboardController(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._orderRepository = orderRepository;
            this._mapper = mapper;
        }

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            var temp = _orderRepository.GetByDateDecending();
            var data = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderVM>>(temp);
           
            ViewBag.OrderModel = data;

            return View();
        }
    }
}