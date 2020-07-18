using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Interfaces;

namespace WebBanSach_2_0.Data.Repositories
{
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
