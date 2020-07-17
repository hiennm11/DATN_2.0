using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetProductByNameID(string nameId);
        Task<Product> GetProductByNameIDAsync(string nameId);
        Task<Product> GetProductByAdderAsync(ProductAdder productAdder, string alias);
        Task<IEnumerable<Product>> GetByCategoryAsync(string cate);
        Task<IEnumerable<Product>> GetBySearchAsync(string search);
        Task<IEnumerable<Product>> GetByCategoryPagingAsync(string cate, int page, int pageSize);
        Task<IEnumerable<Product>> GetBySearchPagingAsync(string search, int page, int pageSize);
        Task<IEnumerable<Product>> GetNewProductAsync();
        Task<IEnumerable<Product>> GetHotProductAsync();
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int cate);
        Task DeleteAsync(int id);
    }
}
