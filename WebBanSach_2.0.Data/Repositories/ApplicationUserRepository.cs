using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Interfaces;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public int CountEmp()
        {
            var list = _dbContext.Users.Where(m => !m.Roles.Select(n => n.RoleId).Contains("user"));
            return list.Count();
        }

        public void Delete(string id)
        {
            var model = _dbContext.Users.Find(id);
            _dbContext.Users.Remove(model);
        }

        public async Task<IEnumerable<ApplicationUser>> GetListUserByRole(IdentityRole role)
        {
            return await _dbContext.Users.Where(m => m.Roles.Select(n => n.RoleId).Contains(role.Id)).Include(m => m.Roles).ToListAsync();
        }

        public ApplicationUser GetUserByUserName(string name)
        {
            return _dbContext.Users.FirstOrDefault(m => m.UserName.Equals(name));
        }
    }
}
