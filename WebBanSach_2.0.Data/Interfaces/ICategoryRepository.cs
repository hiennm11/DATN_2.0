using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task Delete(int id);
        Task<IEnumerable<Category>> GetBySearchAsync(string search);
        Task<IEnumerable<Category>> GetTrueCategoriesAsync();

    }
}
