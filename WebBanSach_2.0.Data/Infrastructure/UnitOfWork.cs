using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Save();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private WebBanSach_2_0DbContext _dbContext;
        private IDbFactory _dbFactory;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this._dbFactory = dbFactory;
        }

        public WebBanSach_2_0DbContext DbContext
        {
            get { return _dbContext ?? (_dbContext = _dbFactory.Init()); }
        }


        public void Save()
        {
            _dbContext.SaveChanges();
        }        
    }
}
