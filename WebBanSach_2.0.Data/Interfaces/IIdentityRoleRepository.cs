using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;

namespace WebBanSach_2_0.Data.Interfaces
{
    public interface IIdentityRoleRepository : IRepository<IdentityRole>
    {
        Task<IEnumerable<IdentityRole>> GetListRoles();
        void Delete(string id);
    }
}
