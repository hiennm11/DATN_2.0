using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Interfaces;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories
{
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
