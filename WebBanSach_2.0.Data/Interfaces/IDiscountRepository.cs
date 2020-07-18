using System.Collections.Generic;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Data.Interfaces
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<IEnumerable<Discount>> GetProductDiscount();
        Task<Discount> GetDiscountById(int id, string[] includes = null);
        Task<Discount> GetDiscountByPromoCode(string code);
    }
}
