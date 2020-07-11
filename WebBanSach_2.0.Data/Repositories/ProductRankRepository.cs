using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories
{
    public interface IProductRankRepository : IRepository<ProductRank>
    {
    }

    public class ProductRankRepository : GenericRepository<ProductRank>, IProductRankRepository
    {
        public ProductRankRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }
    }
}
