using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories.Interfaces;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories
{

    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<Comment>> GetListCommentByProductId(int productId)
        {
            return await _dbContext.Comments.Where(m => m.ProductId == productId).Include(n => n.User).ToListAsync();
        }
    }
}
