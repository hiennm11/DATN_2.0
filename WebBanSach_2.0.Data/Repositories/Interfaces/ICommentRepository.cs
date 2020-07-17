using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetListCommentByProductId(int productId);
    }
}
