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
    public interface IAuthorDetailRepository : IRepository<Author>
    {
        Task Delete(int id);
        Task<Author> GetAuthorById(int id);
        Task<IEnumerable<Author>> GetBySearchAsync(string nameID);
    }
    public class AuthorDetailRepository : GenericRepository<Author>, IAuthorDetailRepository
    {
        public AuthorDetailRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public async Task Delete(int id)
        {
            var obj =_dbContext.Authors.Find(id);
            obj.Status = false;
            await this.UpdateAsync(obj);
        }

        public async Task<IEnumerable<Author>> GetBySearchAsync(string search)
        {
            var list = _dbContext.Authors.Where(m => m.Name.ToLower().Contains(search.ToLower()));
            return await list.ToListAsync();
        }

        public async Task<Author> GetAuthorById(int id)
        {
            var model = await this.GetSingleByIDAsync(id);
            await _dbContext.Entry(model).Collection(p => p.Products).LoadAsync();
            return model;
        }

    }
}
