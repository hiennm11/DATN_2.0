using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.Enums;
using WebBanSach_2_0.Model.ReportModels;

namespace WebBanSach_2_0.Data.Repositories
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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private int _filterRow;
        public OrderRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetByDateDecending()
        {
            return await _dbContext.Orders.OrderByDescending(m => m.CreatedDate).OrderBy(m => m.Status).Take(10).ToListAsync();
        }

        public async Task<Order> GetByOrderIdAsync(int id)
        {
            return await _dbContext.Orders.Include(m => m.Discount).FirstOrDefaultAsync(m => m.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetCompletedOrder(DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 10)
        {
            var data = _dbContext.Orders.Where(m => m.Status == OrderStatus.Completed).OrderByDescending(m => m.CreatedDate)
                        .AsQueryable();
            if (fromDate != null || toDate != null)
            {
                if (fromDate == null)
                {
                    data = data.Where(m => m.CreatedDate <= toDate);
                }
                else if (toDate == null)
                {
                    data = data.Where(m => m.CreatedDate >= fromDate);
                }
                else
                {
                    data = data.Where(m => m.CreatedDate >= fromDate && m.CreatedDate <= toDate);
                }
            }

            _filterRow = data.Count();

            return await data.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetDashboardListOrder()
        {
            return await _dbContext.Orders.Where(m => m.Status == OrderStatus.Waiting || m.Status == OrderStatus.InProgress ||
                                                 m.Status == OrderStatus.Shipping || m.Status == OrderStatus.Deliveried ||
                                                 m.Status == OrderStatus.Completed || m.Status == OrderStatus.Completed)
                                                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetDeletedOrder(DateTime? fromDate, DateTime? toDate, int? orderStatus, int page = 1, int pageSize = 10)
        {
            var data = _dbContext.Orders.Where(m => m.Status == OrderStatus.Declined || m.Status == OrderStatus.Deleted
                                               || m.Status == OrderStatus.Cancelled).OrderByDescending(m => m.CreatedDate).AsQueryable();
            if (fromDate != null || toDate != null)
            {
                if (fromDate == null)
                {
                    data = data.Where(m => m.CreatedDate <= toDate);
                }
                else if (toDate == null)
                {
                    data = data.Where(m => m.CreatedDate >= fromDate);
                }
                else
                {
                    data = data.Where(m => m.CreatedDate >= fromDate && m.CreatedDate <= toDate);
                }
            }

            if (orderStatus != null)
            {
                data = data.Where(m => m.Status == (OrderStatus)orderStatus);
            }

            _filterRow = data.Count();

            return await data.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public int GetFilterRow()
        {
            return _filterRow;
        }

        public async Task<IEnumerable<Order>> GetListByMonthAsync(DateTime date)
        {
            return await _dbContext.Orders.Where(m => m.CreatedDate.Value.Month == date.Month && 
                                                      m.CreatedDate.Value.Year == date.Year)
                                          .Include(m => m.Discount).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetListByOrderDateAsync(DateTime date)
        {
            return await _dbContext.Orders.Where(m => m.CreatedDate.Value.Day == date.Day && 
                                                      m.CreatedDate.Value.Month == date.Month && 
                                                      m.CreatedDate.Value.Year == date.Year)
                                          .Include(m => m.Discount).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetListOrder(DateTime? fromDate, DateTime? toDate, int? orderStatus, int page = 1, int pageSize = 10)
        {
            var data = _dbContext.Orders.AsQueryable();
            if (fromDate != null || toDate != null)
            {
                if (fromDate == null)
                {
                    data = data.Where(m => m.CreatedDate <= toDate);
                }
                else if (toDate == null)
                {
                    data = data.Where(m => m.CreatedDate >= fromDate);
                }
                else
                {
                    data = data.Where(m => m.CreatedDate >= fromDate && m.CreatedDate <= toDate);
                }
            }

            if(orderStatus != null)
            {
                data = data.Where(m => m.Status == (OrderStatus)orderStatus);
            }

            _filterRow = data.Count();

            return await data.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public DbRawSqlQuery<OrderDetailRM> GetOrderDetailStoreProcedure(int orderId)
        {
            return _dbContext.Database.SqlQuery<OrderDetailRM>("SelectAllProductInOrder @OrderId", new SqlParameter("@OrderId", orderId));
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string email)
        {
            var result = _dbContext.Orders.Where(m => m.User.UserName == email).Include(m => m.Discount);
            return await result.ToListAsync();
        }

        public DbRawSqlQuery<OrderRM> GetOrderStoreProcedure(int orderId)
        {
            return _dbContext.Database.SqlQuery<OrderRM>("SelectOrder @OrderId", new SqlParameter("@OrderId", orderId));
        }

        public async Task<IEnumerable<Order>> GetUnDoneOrder(DateTime? fromDate, DateTime? toDate, int? orderStatus, int page = 1, int pageSize = 10)
        {
            var data = _dbContext.Orders.Where(m => m.Status == OrderStatus.InProgress || m.Status == OrderStatus.Accepted
                                               || m.Status == OrderStatus.Shipping || m.Status == OrderStatus.Deliveried).
                                               OrderByDescending(m => m.CreatedDate).AsQueryable();
            if (fromDate != null || toDate != null)
            {
                if (fromDate == null)
                {
                    data = data.Where(m => m.CreatedDate <= toDate);
                }
                else if (toDate == null)
                {
                    data = data.Where(m => m.CreatedDate >= fromDate);
                }
                else
                {
                    data = data.Where(m => m.CreatedDate >= fromDate && m.CreatedDate <= toDate);
                }
            }

            if (orderStatus != null)
            {
                data = data.Where(m => m.Status == (OrderStatus)orderStatus);
            }

            _filterRow = data.Count();

            return await data.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetWaitingOrder(DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 10)
        {
            var data = _dbContext.Orders.Where(m => m.Status == OrderStatus.Waiting).OrderByDescending(m => m.CreatedDate).AsQueryable();
            if (fromDate != null || toDate != null)
            {
                if (fromDate == null)
                {
                    data = data.Where(m => m.CreatedDate <= toDate);
                }
                else if (toDate == null)
                {
                    data = data.Where(m => m.CreatedDate >= fromDate);
                }
                else
                {
                    data = data.Where(m => m.CreatedDate >= fromDate && m.CreatedDate <= toDate);
                }
            }

            _filterRow = data.Count();

            return await data.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
