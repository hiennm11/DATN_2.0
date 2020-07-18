using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Interfaces;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories
{
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
            var model = await _dbContext.Products.FirstOrDefaultAsync(n => n.NameAlias == nameId);
            await _dbContext.Entry(model).Collection(p => p.Authors).LoadAsync();
            await _dbContext.Entry(model).Reference(p => p.Category).LoadAsync();
            await _dbContext.Entry(model).Reference(p => p.Discount).LoadAsync();

            return model;
        }

        public async Task<IEnumerable<Product>> GetBySearchAsync(string search)
        {            
            var list = _dbContext.Products.Where(m => m.NameAlias.ToLower().Contains(search.ToLower()));
            return await list.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string cate)
        {
            var list = _dbContext.Products.Where(c => c.Category.NameAlias.Equals(cate)).OrderBy(c => c.Name);
            return await list.Where(m => m.Status == true).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int cate)
        {
            var list = _dbContext.Products.Where(c => c.Category.CategoryId == cate).OrderBy(c => c.Name).Include(m => m.Discount);
            return await list.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetNewProductAsync()
        {
            var list = _dbContext.Products.Where(m => m.Status == true).OrderBy(c => c.UpdatedDate).Include(n => n.Discount).Take(16);
            return await list.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetHotProductAsync()
        {
            var lst = _dbContext.ProductRanks.ToList();
            var list = _dbContext.Products.Where(m => m.Status == true).OrderBy(c => c.UpdatedDate).Include(n => n.Discount).Take(16);
            return await list.ToListAsync();
        }

        public Product GetProductByNameID(string nameId)
        {
            return _dbContext.Products.Include(m => m.Discount).FirstOrDefault(n => n.NameAlias == nameId);
        }

        public async Task<IEnumerable<Product>> GetByCategoryPagingAsync(string cate, int page, int pageSize)
        {
            var list = _dbContext.Products.Where(c => c.Category.NameAlias == cate).OrderBy(c => c.Name);
            return await list.Where(m => m.Status == true).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetBySearchPagingAsync(string search, int page, int pageSize)
        {
            var list = _dbContext.Products.Where(m => m.NameAlias.ToLower().Contains(search.ToLower())).OrderBy(c => c.Name);
            return await list.Where(m => m.Status == true).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<Product> GetProductByAdderAsync(ProductAdder productAdder, string alias)
        {
            return _dbContext.Products.FirstOrDefaultAsync(m => m.ProductId == productAdder.ProductId && 
                                                           m.NameAlias == alias && m.CategoryId == productAdder.CategoryId);
        }
    }
}
