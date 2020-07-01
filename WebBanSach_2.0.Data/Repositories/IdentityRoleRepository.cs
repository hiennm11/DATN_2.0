using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;

namespace WebBanSach_2_0.Data.Repositories
{
    public interface IIdentityRoleRepository : IRepository<IdentityRole>
    {
        Task<IEnumerable<IdentityRole>> GetListRoles();
        void Delete(string id);
    }
    class IdentityRoleRepository : GenericRepository<IdentityRole>, IIdentityRoleRepository
    {
        public IdentityRoleRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public void Delete(string id)
        {
            var item = _dbContext.Roles.Find(id);
            _dbContext.Roles.Remove(item);
        }

        public async Task<IEnumerable<IdentityRole>> GetListRoles()
        {
            return await _dbContext.Roles.Where(m => !m.Id.Equals("user")).ToListAsync();
        }
    }
}
