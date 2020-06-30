using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Data.Repositories
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        Task<IEnumerable<OrderDetail>> GetDetailByOrderId(int id);
    }
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<OrderDetail>> GetDetailByOrderId(int id)
        {
            return await _dbContext.OrderDetails.Where(m => m.OrderId == id).Include(n => n.Product).Include(n => n.Product.Discount).ToListAsync();
        }
    }
}
