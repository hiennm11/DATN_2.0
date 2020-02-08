using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;

namespace WebBanSach_2_0.Data.Repositories
{
    public interface IProductRepository
    {
        Product GetProductByNameID(string nameId);
        IEnumerable<Product> GetAllPaging(int page, int pageSize);
        void Delete(int id);
    }
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public void Delete(int id)
        {
            var product = _dbContext.Products.Find(id);
            product.Status = false;
            this.Update(product);
        }

        public Product GetProductByNameID(string nameId)
        {
            return _dbContext.Products.FirstOrDefault(n => n.NameID == nameId);
        }

        public IEnumerable<Product> GetAllPaging(int page, int pageSize)
        {
            return _dbContext.Products.OrderBy(c => c.Name).Skip((page - 1) * pageSize).Take(pageSize);
        }
        
    }
}
