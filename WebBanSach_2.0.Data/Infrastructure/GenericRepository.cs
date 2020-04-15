using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Data.Infrastructure
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected WebBanSach_2_0DbContext _dbContext;
        protected readonly IDbSet<T> _dbSet;
      
        public GenericRepository(WebBanSach_2_0DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
            return await Task.Run(() => _dbSet.Add(entity));
        }

        public int Count(Expression<Func<T, bool>> where)
        {
            return _dbSet.Count(where);
        }

        public async Task<IEnumerable<T>> GetAllAsync(string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = _dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return await query.ToListAsync();
            }

            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetSingleByIDAsync(int id)
        {
            return await Task.Run(() => _dbSet.Find(id));
        }

        public async Task<T> GetSingleByStringIDAsync(string id)
        {
            return await Task.Run(() => _dbSet.Find(id));
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() =>
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            });
        }

        public async Task<T> ShiftDeleteAsync(int id)
        {
            return await Task.Run(() =>
            {
                var entity = _dbSet.Find(id);
                return _dbSet.Remove(entity);
            });
        }

        public async Task<IEnumerable<T>> GetPagingAsync(int page, int pageSize)
        {
            return await _dbContext.Set<T>().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
