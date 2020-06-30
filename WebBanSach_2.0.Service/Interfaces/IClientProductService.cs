using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.Interfaces
{
    public interface IClientProductService
    {
        Task<IEnumerable<ProductVM>> GetProductsByCategoryId(int id);
        Task<IndexViewModel<ProductVM>> GetAllProducts(string categoryId = null, string search = null, int pageSize = 16, int page = 1);
        Task<IEnumerable<ProductVM>> GetHotProducts();
        Task<IEnumerable<ProductVM>> GetNewProducts();
        Task<ProductVM> GetProductByNameAlias(string id);
    }
}
