using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser> 
    {
        void Delete(string id);
        ApplicationUser GetUserByUserName(string name);
    }
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }

        public void Delete(string id)
        {
            var model = _dbContext.Users.Find(id);
            _dbContext.Users.Remove(model);
        }

        public ApplicationUser GetUserByUserName(string name)
        {
            return _dbContext.Users.FirstOrDefault(m => m.UserName.Equals(name));
        }
    }
}
