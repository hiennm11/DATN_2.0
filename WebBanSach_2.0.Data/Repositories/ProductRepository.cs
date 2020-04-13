using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;

namespace WebBanSach_2_0.Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetProductByNameID(string nameId);
        IEnumerable<Product> GetByCategory(string cate);
        IEnumerable<Product> GetNewProduct();
        IEnumerable<Product> GetHotProduct();
        IEnumerable<Product> GetTrueProduct();
        IEnumerable<Product> GetBySearch(string search);
        IEnumerable<Product> GetByCategoryInt(int cate);
        Task Delete(int id);
    }
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public async Task Delete(int id)
        {
            var product = _dbContext.Products.Find(id);
            product.Status = false;
            await this.Update(product);
        }

        public Product GetProductByNameID(string nameId)
        {
            return _dbContext.Products.FirstOrDefault(n => n.NameID == nameId);
        }

        public IEnumerable<Product> GetBySearch(string search)
        {
            var list = _dbContext.Products.Where(m => m.NameID.ToLower().Contains(search.ToLower()));
            return list;
        }

        public IEnumerable<Product> GetByCategory(string cate)
        {
            var list = _dbContext.Products.Where(c => c.Categories.Description == cate).OrderBy(c => c.Name);
            return list;
        }
        public IEnumerable<Product> GetByCategoryInt(int cate)
        {
            var list = _dbContext.Products.Where(c => c.Categories.ID == cate).OrderBy(c => c.Name);
            return list;
        }

        public IEnumerable<Product> GetNewProduct()
        {
            var list = _dbContext.Products.OrderBy(c => c.CreateDate);
            return list;
        }

        public IEnumerable<Product> GetHotProduct()
        {
            var list = _dbContext.Products.OrderBy(c => c.UpdatedDate);
            return list;
        }

        public IEnumerable<Product> GetTrueProduct()
        {
            var list = _dbContext.Products.OrderBy(c => c.Status);
            return list;
        }
    }
}
