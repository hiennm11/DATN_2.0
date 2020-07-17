using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Repositories.Interfaces
{
    public interface IProductAdderRepository : IRepository<ProductAdder>
    {
        Task<IEnumerable<ProductAdder>> GetByCategoryAsync(int cate);
        Task<IEnumerable<ProductAdder>> GetBySearchAsync(string search);
        Task<double> GetImportCostByProductId(int productId);
        Task<double> GetImportCostByDate(DateTime date);
        Task<double> GetImportCostByMonth(DateTime date);
        Task<ProductAdder> GetByIdAsync(int id);
    }
}
