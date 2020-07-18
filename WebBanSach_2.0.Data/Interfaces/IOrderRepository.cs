using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ReportModels;

namespace WebBanSach_2_0.Data.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetListOrder(DateTime? fromDate, DateTime? toDate, int? orderStatus, int page = 1, int pageSize = 10);
        Task<IEnumerable<Order>> GetDeletedOrder(DateTime? fromDate, DateTime? toDate, int? orderStatus, int page = 1, int pageSize = 10);
        Task<IEnumerable<Order>> GetWaitingOrder(DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 10);
        Task<IEnumerable<Order>> GetCompletedOrder(DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 10);
        Task<IEnumerable<Order>> GetUnDoneOrder(DateTime? fromDate, DateTime? toDate, int? orderStatus, int page = 1, int pageSize = 10);
        Task<IEnumerable<Order>> GetByDateDecending();
        Task<Order> GetByOrderIdAsync(int id);
        Task<IEnumerable<Order>> GetListByOrderDateAsync(DateTime date);
        Task<IEnumerable<Order>> GetListByMonthAsync(DateTime date);
        Task<IEnumerable<Order>> GetOrdersByUserAsync(string email);
        Task<IEnumerable<Order>> GetDashboardListOrder();
        DbRawSqlQuery<OrderRM> GetOrderStoreProcedure(int orderId);
        DbRawSqlQuery<OrderDetailRM> GetOrderDetailStoreProcedure(int orderId);

        int GetFilterRow();
    }
}
