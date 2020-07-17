using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories.Interfaces
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        void Delete(string id);
        ApplicationUser GetUserByUserName(string name);
        Task<IEnumerable<ApplicationUser>> GetListUserByRole(IdentityRole role);
        int CountEmp();
    }
}
