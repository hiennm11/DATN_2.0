using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Models;

namespace WebBanSach_2_0.Data.Repositories
{
    public interface ILoaiSachRepository : IRepository<LoaiSach>
    {

    }
    public class LoaiSachRepository : RepositoryBase<LoaiSach>, ILoaiSachRepository
    {
        public LoaiSachRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
