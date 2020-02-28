using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
        public T Add(T entity)
        {
            return _dbSet.Add(entity);
        }

        public int Count(Expression<Func<T, bool>> where)
        {
            return _dbSet.Count(where);
        }

        public async Task<IEnumerable<T>> GetAll(string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = _dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.AsQueryable();
            }

            return _dbContext.Set<T>().AsQueryable();
        }

        public async Task<T> GetSingleByID(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> GetSingleByStringID(string id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public T ShiftDelete(int id)
        {
            var entity = _dbSet.Find(id);
            return _dbSet.Remove(entity);
        }
    }
}
