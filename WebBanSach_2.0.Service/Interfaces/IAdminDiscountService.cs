using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.Interfaces
{
    public interface IAdminDiscountService
    {
        Task<int> AddProductToDiscount(int discountId, int[] productId);
        Task<int> DeleteProductFromDiscount(int discountId, int productId);
        Task<IndexViewModel<DiscountVM>> GetDataAsync(string search, int pageSize, int page);
        Task<DiscountVM> GetDataByIDAsync(int id);
        Task<int> SaveDataAsync(DiscountVM viewModel);
        Task<int> DeleteDataAsync(int id);
    }
}
