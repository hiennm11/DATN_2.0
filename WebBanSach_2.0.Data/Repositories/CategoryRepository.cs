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
    public interface ICategoryRepository : IRepository<Category>
    {
        Task Delete(int id);
        Task<IEnumerable<Category>> GetBySearchAsync(string search);
        Task<IEnumerable<Category>> GetTrueCategoriesAsync();

    }
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Category>> GetTrueCategoriesAsync()
        {
            var result = _dbContext.Categories.OrderBy(c => c.Status);
            return await result.ToListAsync();
        }

        public async Task Delete(int id)
        {
            var cate = _dbContext.Categories.Find(id);
            cate.Status = false;
            await this.UpdateAsync(cate);
        }

        public async Task<IEnumerable<Category>> GetBySearchAsync(string search)
        {
            var list = _dbContext.Categories.Where(m => m.CategoryName.ToLower().Contains(search.ToLower()));
            return await list.ToListAsync();
        }
    }
}
