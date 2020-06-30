using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<Discount> GetDiscountById(int id, string[] includes = null);
        Task<Discount> GetDiscountByPromoCode(string code);
    }

    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {

        }

        public async Task<Discount> GetDiscountById(int id, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = _dbContext.Discounts.Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return await query.FirstOrDefaultAsync(m => m.DiscountId == id);
            }

            return await _dbContext.Discounts.FirstOrDefaultAsync(m => m.DiscountId == id);
        }

        public async Task<Discount> GetDiscountByPromoCode(string code)
        {
            var model =  await _dbContext.Discounts.FirstOrDefaultAsync(m => m.DiscountCode.Equals(code));
            if(!model.DiscountCode.Equals(code))
            {
                return null;
            }
            return model;
        }
    }
}
