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
    public class ManageOrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public ManageOrderController(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._orderRepository = orderRepository;
            this._mapper = mapper;
        }

        // GET: Admin/ManageOrder
        public async Task<ActionResult> Index(int page = 1)
        {
            var temp = await _orderRepository.GetAllAsync();
            var data = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderVM>>(temp);
            var pager = new Pager(data.Count(), page);

            var viewModel = new IndexViewModel<OrderVM>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            ViewBag.OrderModel = viewModel;
            return View();
        }

        public async Task<ActionResult> UpdateStatus(int id, int orderStatus)
        {
           bool status = false; string message = String.Empty;
           var item = await _orderRepository.GetSingleByIDAsync(id);
            if(item != null)
            {
                item.Status = orderStatus;
                await _orderRepository.UpdateAsync(item);
                try
                {
                    await _unitOfWork.SaveAsync();
                    status = true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;                    
                }
            }
            return Json(new { status = status, message = message });
        }
    }
}