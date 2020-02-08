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
        void Delete(int id);
    }
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public void Delete(int id)
        {
            var cate = _dbContext.Categories.Find(id);
            cate.Status = false;
            this.Update(cate);
        }    
    }
}
