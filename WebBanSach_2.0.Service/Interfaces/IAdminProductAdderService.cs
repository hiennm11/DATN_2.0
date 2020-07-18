using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.Interfaces
{
    public interface IAdminProductAdderService
    {
        Task<IndexViewModel<ProductAdderVM>> GetDataAsync(int page, int pageSize, int? categoryId, string search = null);
        Task<ProductAdderVM> GetDataByIDAsync(int id);
        Task<int> SaveDataAsync(ProductAdderVM viewModel);

    }
}
