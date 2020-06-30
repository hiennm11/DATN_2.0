using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.Interfaces
{
    public interface IAdminProductService
    {
        Task<int> AddAuthorToProduct(string productId, int[] authorId);
        Task<int> DeleteAuthorFromProduct(string productId, int authorId);
        Task<IEnumerable<ProductVM>> GetAllProductAsync();
        Task<IndexViewModel<ProductVM>> GetDataAsync(int page, string search, int? cateId);
        Task<IEnumerable<CategoryVM>> GetCategoriesListAsync();
        Task<ProductVM> GetDataByIDAsync(string id);
        Task<int> SaveDataAsync(ProductVM viewModel);
        Task<int> DeleteDataAsync(int id);
    }
}
