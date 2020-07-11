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
    public interface IProductAdderRepository : IRepository<ProductAdder>
    {
        Task<IEnumerable<ProductAdder>> GetByCategoryAsync(int cate);
        Task<IEnumerable<ProductAdder>> GetBySearchAsync(string search);
        Task<double> GetImportCostByProductId(int productId);
        Task<double> GetImportCostByDate(DateTime date);
        Task<double> GetImportCostByMonth(DateTime date);
        Task<ProductAdder> GetByIdAsync(int id);
    }

    public class ProductAdderRepository : GenericRepository<ProductAdder>, IProductAdderRepository
    {
        public ProductAdderRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<ProductAdder>> GetByCategoryAsync(int cate)
        {
            return await _dbContext.ProductAdders.Where(m => m.CategoryId == cate).ToListAsync();
        }

        public Task<ProductAdder> GetByIdAsync(int id)
        {
            return _dbContext.ProductAdders.FirstOrDefaultAsync(m => m.AdderId == id);
        }

        public async Task<IEnumerable<ProductAdder>> GetBySearchAsync(string search)
        {
            return await _dbContext.ProductAdders.Where(m => m.Name.Contains(search)).ToListAsync();
        }

        public async Task<double> GetImportCostByDate(DateTime date)
        {
            var list =  _dbContext.ProductAdders.Where(m => m.PurchaseDate.Date.Equals(date.Date));
            return await list.SumAsync(m => m.Price * m.AvailableQuantity);
        }

        public async Task<double> GetImportCostByMonth(DateTime date)
        {
            var list = _dbContext.ProductAdders.Where(m => m.PurchaseDate.Date.Month.Equals(date.Date.Month)
                                                        && m.PurchaseDate.Date.Year.Equals(date.Date.Year));
            return await list.SumAsync(m => m.Price * m.AvailableQuantity);
        }

        public async Task<double> GetImportCostByProductId(int productId)
        {
            var list = _dbContext.ProductAdders.Where(m => m.ProductId.Equals(productId));
            return await list.SumAsync(m => m.Price * m.AvailableQuantity);
        }
    }
}
