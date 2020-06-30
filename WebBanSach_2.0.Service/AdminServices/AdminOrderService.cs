using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.Enums;
using WebBanSach_2_0.Model.ResponseModels;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.Enums;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.AdminServices
{
    public interface IAdminOrderService
    {
        Task<int> ChangeOrderStatus(int orderId);
        Task<int> ChangeOrderStatusToFalse(int orderId);
        Task<IndexViewModel<OrderVM>> GetDataAsync(DateTime? fromDate, DateTime? toDate, int? orderStatus, int page, OrderTypeRequest orderType);
        Task<IEnumerable<OrderVM>> GetOrder(string userEmail);
        Task<IEnumerable<OrderVM>> GetOrderByDateDecending();
        Task<ClientOrderDetailResponse> GetOrderDetailCartView(int id);
        Task<AdminChartResponse> GetChartResponse();
        Task<double> GetDayEarning(DateTime date);
    }

    public class AdminOrderService : IAdminOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;

        public AdminOrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._mapper = mapper;
        }

        public async Task<int> ChangeOrderStatus(int orderId)
        {
            var model = await _orderRepository.GetByOrderIdAsync(orderId);
            var sttInt = (int)model.Status + 1;
            model.Status = (OrderStatus)sttInt;
            await _orderRepository.UpdateAsync(model);
            return await _unitOfWork.SaveAsync();
        }

        public async Task<int> ChangeOrderStatusToFalse(int orderId)
        {
            var model = await _orderRepository.GetByOrderIdAsync(orderId);

            switch (model.Status)
            {
                case OrderStatus.Waiting: model.Status = OrderStatus.Declined; break;
                case OrderStatus.Deliveried: model.Status = OrderStatus.Refunded; break;
                case OrderStatus.Completed: model.Status = OrderStatus.Deleted; break;
                default: model.Status = OrderStatus.Cancelled; break;
            }
            
            await _orderRepository.UpdateAsync(model);
            return await _unitOfWork.SaveAsync();
        }

        public Task<AdminChartResponse> GetChartResponse()
        {
            throw new NotImplementedException();
        }

        public async Task<IndexViewModel<OrderVM>> GetDataAsync(DateTime? fromDate, DateTime? toDate, int? orderStatus, int page, OrderTypeRequest orderType)
        {
            var data = new List<OrderVM>().AsEnumerable();
            var pager = new Pager(_orderRepository.GetTotalRow(), page);

            switch (orderType)
            {
                case OrderTypeRequest.WaitingOrder:
                    data = _mapper.Map<IEnumerable<OrderVM>>(await _orderRepository.GetWaitingOrder(fromDate, toDate, page));
                    pager = new Pager(_orderRepository.GetFilterRow(), page); break;
                case OrderTypeRequest.UndoneOrder:
                    data = _mapper.Map<IEnumerable<OrderVM>>(await _orderRepository.GetUnDoneOrder(fromDate, toDate, orderStatus, page));
                    pager = new Pager(_orderRepository.GetFilterRow(), page); break;
                case OrderTypeRequest.CompletedORder:
                    data = _mapper.Map<IEnumerable<OrderVM>>(await _orderRepository.GetCompletedOrder(fromDate, toDate, page));
                    pager = new Pager(_orderRepository.GetFilterRow(), page); break;
                default:
                    data = _mapper.Map<IEnumerable<OrderVM>>(await _orderRepository.GetDeletedOrder(fromDate, toDate, orderStatus, page));
                    pager = new Pager(_orderRepository.GetFilterRow(), page); break;
            }

            return new IndexViewModel<OrderVM>()
            {
                Items = data,
                Pager = pager
            };
        }

        public async Task<double> GetDayEarning(DateTime date)
        {
            var detail = new List<ClientOrderDetailResponse>();
            var orders = _mapper.Map<IEnumerable<OrderVM>>(await _orderRepository.GetListByOrderDateAsync(date));
            foreach(var item in orders)
            {
                var list = _mapper.Map<IEnumerable<OrderDetailVM>>(await _orderDetailRepository.GetDetailByOrderId(item.OrderId));
                detail.Add(new ClientOrderDetailResponse(list.ToList(), item));
            }

            return detail.Sum(m => m.TotalPrice + 50000);
        }

        public async Task<IEnumerable<OrderVM>> GetOrder(string userEmail)
        {
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderVM>>(await _orderRepository.GetOrdersByUserEmailAsync(userEmail));
        }

        public async Task<IEnumerable<OrderVM>> GetOrderByDateDecending()
        {
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderVM>>(await _orderRepository.GetByDateDecending());
        }

        public async Task<ClientOrderDetailResponse> GetOrderDetailCartView(int id)
        {
            var list = _mapper.Map<IEnumerable<OrderDetailVM>>(await _orderDetailRepository.GetDetailByOrderId(id));
            var order = _mapper.Map<OrderVM>(await _orderRepository.GetByOrderIdAsync(id));

            return new ClientOrderDetailResponse(list.ToList(), order);
        }
    }
}
