using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.Interfaces
{
    public interface IAdminCategoryService
    {
        Task<IndexViewModel<CategoryVM>> GetDataAsync(int page, string search);
        Task<CategoryVM> GetDataByIDAsync(int id);
        Task<int> SaveDataAsync(CategoryVM viewModel);
        Task<int> DeleteDataAsync(int id);
    }
}
