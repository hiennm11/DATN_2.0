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
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductByNameIDAsync(string nameId);
        Task<IEnumerable<Product>> GetByCategoryAsync(string cate);
        Task<IEnumerable<Product>> GetNewProductAsync();
        Task<IEnumerable<Product>> GetHotProductAsync();
        Task<IEnumerable<Product>> GetBySearchAsync(string search);
        Task<IEnumerable<Product>> GetByCategoryIntAsync(int cate);
        Task DeleteAsync(int id);
    }
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public async Task DeleteAsync(int id)
        {
            var product = _dbContext.Products.Find(id);
            product.Status = false;
            await this.UpdateAsync(product);
        }

        public async Task<Product> GetProductByNameIDAsync(string nameId)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(n => n.NameID == nameId);
        }

        public async Task<IEnumerable<Product>> GetBySearchAsync(string search)
        {
            var list = _dbContext.Products.Where(m => m.NameID.ToLower().Contains(search.ToLower()));
            return await list.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string cate)
        {
            var list = _dbContext.Products.Where(c => c.Category.Description == cate).OrderBy(c => c.Name);
            return await list.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetByCategoryIntAsync(int cate)
        {
            var list = _dbContext.Products.Where(c => c.Category.CategoryId == cate).OrderBy(c => c.Name);
            return await list.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetNewProductAsync()
        {
            var list = _dbContext.Products.OrderBy(c => c.CreateDate);
            return await list.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetHotProductAsync()
        {
            var list = _dbContext.Products.OrderBy(c => c.UpdatedDate);
            return await list.ToListAsync();
        }
    }
}
