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
        IAuthorDetailRepository AuthorDetail { get; }
        IProductAuthorRepository ProductAuthor { get; }
        IIdentityRoleRepository IdentityRole { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        
        Task Save();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebBanSach_2_0DbContext _dbContext;

        public UnitOfWork(WebBanSach_2_0DbContext dbContext)
        {
            _dbContext = dbContext;
            Category = new CategoryRepository(_dbContext);
            Product = new ProductRepository(_dbContext);
            AuthorDetail = new AuthorDetailRepository(_dbContext);
            ProductAuthor = new ProductAuthorRepository(_dbContext);
            IdentityRole = new IdentityRoleRepository(_dbContext);
            ApplicationUser = new ApplicationUserRepository(_dbContext);
            OrderRepository = new OrderRepository(_dbContext);
            OrderDetailRepository = new OrderDetailRepository(_dbContext);
        }

        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IAuthorDetailRepository AuthorDetail { get; private set; }
        public IProductAuthorRepository ProductAuthor { get; private set; }
        public IIdentityRoleRepository IdentityRole { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderRepository OrderRepository { get; private set; }
        public IOrderDetailRepository OrderDetailRepository { get; private set; }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
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
