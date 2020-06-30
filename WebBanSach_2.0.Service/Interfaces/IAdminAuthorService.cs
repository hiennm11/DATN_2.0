using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.Interfaces
{
    public interface IAdminAuthorService
    {
        Task<int> AddProductToAuthor(int authorId, int[] productId);
        Task<int> DeleteProductFromAuthor(int authorId, int productId);
        Task<IEnumerable<AuthorVM>> GetAllAuthorAsync();
        Task<IndexViewModel<AuthorVM>> GetDataAsync(int page, string search);
        Task<AuthorVM> GetDataByIDAsync(int id);
        Task<int> SaveDataAsync(AuthorVM viewModel);
        Task<int> DeleteDataAsync(int id);
    }
}
