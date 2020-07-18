using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.ResponseModels;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.Enums;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.Interfaces
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
        string SaveReportFile(int orderId, string fileName, string reportPath, string filePath, string agent);
    }
}
