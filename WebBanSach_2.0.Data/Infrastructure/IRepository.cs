using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Data.Infrastructure
{
    public interface IRepository<T> where T : class 
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> ShiftDeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(string[] includes = null);
        Task<IEnumerable<T>> GetPagingAsync(int page, int pageSize, string[] includes = null);
        Task<T> GetSingleByIDAsync(int id);
        Task<T> GetSingleByStringIDAsync(string id);
        int Count(Expression<Func<T, bool>> where);
        int GetTotalRow();

    }
}
