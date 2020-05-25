using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories
{
    public interface IProductAuthorRepository : IRepository<ProductAuthor>
    {
        void Delete(int id);
        IEnumerable<IGrouping<string, string>> GetGrouping();
        IEnumerable<ProductAuthor> GetByProduct(int id);
    }
    class ProductAuthorRepository : GenericRepository<ProductAuthor>, IProductAuthorRepository
    {
        public ProductAuthorRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public void Delete(int id)
        {
            var list = _dbContext.ProductAuthors.Where(x => x.ProductId == id);
            foreach(var item in list)
            {
                _dbContext.ProductAuthors.Remove(item);
            }
        }

        public IEnumerable<ProductAuthor> GetByProduct(int id)
        {
            return _dbContext.ProductAuthors.Where(m => m.ProductId == id);
        }

        public IEnumerable<IGrouping<string, string>> GetGrouping()
        {
            return _dbContext.ProductAuthors.GroupBy(m => m.Product.Name, m => m.Author.Name);
        }
    }
}
