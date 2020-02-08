using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Repositories;

namespace WebBanSach_2_0.Data.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IAuthorRepository Author { get; }
        void Save();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebBanSach_2_0DbContext _dbContext;

        public UnitOfWork(WebBanSach_2_0DbContext dbContext)
        {
            _dbContext = dbContext;
            Category = new CategoryRepository(_dbContext);
            Product = new ProductRepository(_dbContext);
            Author = new AuthorRepository(_dbContext);
        }

        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }

        public IAuthorRepository Author { get; private set; }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
