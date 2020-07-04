﻿using AutoMapper;
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
using WebBanSach_2_0.Service.Infrastructure;
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
        Task<AdminDashboardResponse> GetDashboardResponse();
        Task<double> GetDayEarning(DateTime date);
        Task<double> GetMonthEarning(DateTime date);

    }

    public class AdminOrderService : IAdminOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IIdentityRoleRepository _identityRoleRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMapper _mapper;

        public AdminOrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository,
                                 IIdentityRoleRepository identityRoleRepository, IApplicationUserRepository applicationUserRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._identityRoleRepository = identityRoleRepository;
            this._applicationUserRepository = applicationUserRepository;
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

        public async Task<AdminChartResponse> GetChartResponse()
        {
            var week = EntityExtensions.GetWeekDate(DateTime.Now);
            var months = EntityExtensions.GetRevenue(DateTime.Now);
            var roles = await _identityRoleRepository.GetListRoles();

            var areaLabels = new List<string>();
            var areaData = new List<double>();
            var barLabels = new List<string>();
            var barData = new List<double>();
            var pieLabels = new List<string>();
            var pieData = new List<double>();

            foreach (var item in week)
            {
                var price = await GetDayEarning(item);
                areaLabels.Add("'" + item.Date.Day + "/" + item.Date.Month + "'");
                areaData.Add(price);
            }

            foreach (var item in months)
            {
                var price = await GetMonthEarning(item);
                barLabels.Add("'Tháng " + item.Date.Month + "'");
                barData.Add(price);
            }

            var emp = _applicationUserRepository.CountEmp();

            foreach (var item in roles)
            {
                var user = await _applicationUserRepository.GetListUserByRole(item);
                pieLabels.Add("'" + item.Name + "'");
                pieData.Add(Math.Round((double)user.Count() / emp * 100));
            }

            return new AdminChartResponse { AreaChart = new AreaChart { Date = areaLabels, DateEarning = areaData }, 
                                            BarChart = new BarChart { Date = barLabels, DateEarning = barData },
                                            PieChart = new PieChart { Roles = pieLabels, Percentage = pieData } };
        }

        public async Task<AdminDashboardResponse> GetDashboardResponse()
        {
            var orders = await _orderRepository.GetDashboardListOrder();
            var orderCount = orders.Count();
            var waitingOrders = Math.Round((double)orders.Where(m => m.Status == OrderStatus.Waiting).Count() / orderCount * 100);
            var acceptedOrders = Math.Round((double)orders.Where(m => m.Status == OrderStatus.Accepted).Count() / orderCount * 100);
            var inProgressOrders = Math.Round((double)orders.Where(m => m.Status == OrderStatus.InProgress).Count() / orderCount * 100);
            var shippingOrders = Math.Round((double)orders.Where(m => m.Status == OrderStatus.Shipping).Count() / orderCount * 100);
            var deliveredOrders = Math.Round((double)orders.Where(m => m.Status == OrderStatus.Deliveried).Count() / orderCount * 100);
            var completedOrders = Math.Round((double)orders.Where(m => m.Status == OrderStatus.Completed).Count() / orderCount * 100);

            var orderPercentage = Math.Round(orderCount - completedOrders / orderCount * 100);
            var months = EntityExtensions.GetRevenue(DateTime.Now);
            double annual = 0;
            foreach (var item in months)
            {
                annual += await GetMonthEarning(item);
            }

            return new AdminDashboardResponse
            {
                Charts = await GetChartResponse(),
                MonthlyEarnings = await GetMonthEarning(DateTime.Now),
                AnnualEarnings = annual,
                OrderPercentage = orderPercentage,
                WaitingOrderPercentage = waitingOrders,
                InProgressOrderPercentage = inProgressOrders,
                ShippingOrderPercentage = shippingOrders,
                DeliveriedOrderPercentage = deliveredOrders,
                CompletedOrderPercentage = completedOrders,
                AcceptedOrderPercentage = acceptedOrders,
                WaitingOrderCount = orders.Where(m => m.Status == OrderStatus.Waiting).Count()
            };
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

            return detail.Sum(m => m.TotalPrice + 50000 - m.BonusPrice);
        }

        public async Task<double> GetMonthEarning(DateTime date)
        {
            var detail = new List<ClientOrderDetailResponse>();
            var orders = _mapper.Map<IEnumerable<OrderVM>>(await _orderRepository.GetListByMonthAsync(date));
            foreach (var item in orders)
            {
                var list = _mapper.Map<IEnumerable<OrderDetailVM>>(await _orderDetailRepository.GetDetailByOrderId(item.OrderId));
                detail.Add(new ClientOrderDetailResponse(list.ToList(), item));
            }

            return detail.Sum(m => m.TotalPrice + 50000 - m.BonusPrice);
        }

        public async Task<IEnumerable<OrderVM>> GetOrder(string userEmail)
        {
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderVM>>(await _orderRepository.GetOrdersByUserAsync(userEmail));
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
