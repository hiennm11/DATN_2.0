using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;

namespace WebBanSach_2_0.Data.Repositories
{
        public interface IErrorRepository : IRepository<Error>
        {

        }
    public class ErrorRepository : GenericRepository<Error>, IErrorRepository
    {
        public ErrorRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }
    }
}
