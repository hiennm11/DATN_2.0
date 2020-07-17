using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories.Interfaces
{
    public interface IAuthorDetailRepository : IRepository<Author>
    {
        Task Delete(int id);
        Task<Author> GetAuthorById(int id);
        Task<IEnumerable<Author>> GetBySearchAsync(string nameID);
    }
}
