using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;

namespace WebBanSach_2_0.Data.Repositories
{
    public interface IAuthorDetailRepository : IRepository<AuthorDetail>
    {
        void Delete(int id);
        IEnumerable<AuthorDetail> GetBySearchAsync(string nameID);
    }
    public class AuthorDetailRepository : GenericRepository<AuthorDetail>, IAuthorDetailRepository
    {
        public AuthorDetailRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public void Delete(int id)
        {
            var obj =_dbContext.AuthorDetails.Find(id);
            obj.Status = false;
            this.Update(obj);
        }

        public IEnumerable<AuthorDetail> GetBySearchAsync(string search)
        {
            var list = _dbContext.AuthorDetails.Where(m => m.Name.ToLower().Contains(search.ToLower()));
            return list;
        }
    }
}
