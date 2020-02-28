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
        T Add(T entity);
        void Update(T entity);
        T ShiftDelete(int id);

        Task<IEnumerable<T>> GetAll(string[] includes = null);
        Task<T> GetSingleByID(int id);
        Task<T> GetSingleByStringID(string id);
        int Count(Expression<Func<T, bool>> where);

    }
}
