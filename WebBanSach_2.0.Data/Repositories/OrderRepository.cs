using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.Enums;

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

        Task<IEnumerable<Order>> GetOrdersByUserEmailAsync(string email);
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

        public async Task<IEnumerable<Order>> GetListByOrderDateAsync(DateTime date)
        {
            return await _dbContext.Orders.Where(m => m.CreatedDate.Value.Date.Equals(date.Date)).ToListAsync();
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

        public async Task<IEnumerable<Order>> GetOrdersByUserEmailAsync(string email)
        {
            var result = _dbContext.Orders.Where(m => m.CustomerEmail == email).Include(m => m.Discount);
            return await result.ToListAsync();
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
