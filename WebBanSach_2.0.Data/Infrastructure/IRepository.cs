using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Data.Infrastructure
{
    public interface IRepository<T> where T : class 
    {
        Task<T> Add(T entity);
        Task Update(T entity);
        Task<T> ShiftDelete(int id);

        Task<IEnumerable<T>> GetAll(string[] includes = null);
        Task<IEnumerable<T>> GetPaging(int page, int pageSize);
        Task<T> GetSingleByID(int id);
        Task<T> GetSingleByStringID(string id);
        int Count(Expression<Func<T, bool>> where);

    }
}
