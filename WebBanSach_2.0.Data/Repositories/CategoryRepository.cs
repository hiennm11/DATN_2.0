using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;

namespace WebBanSach_2_0.Data.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task Delete(int id);
        IEnumerable<Category> GetBySearch(string search);
        IEnumerable<Category> GetTrueCategories();

    }
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Category> GetTrueCategories()
        {
            var list = _dbContext.Categories.OrderBy(c => c.Status);
            return list;
        }

        public async Task Delete(int id)
        {
            var cate = _dbContext.Categories.Find(id);
            cate.Status = false;
            await this.UpdateAsync(cate);
        }

        public IEnumerable<Category> GetBySearch(string search)
        {
            var list = _dbContext.Categories.Where(m => m.CategoryName.ToLower().Contains(search.ToLower()));
            return list;
        }
    }
}
