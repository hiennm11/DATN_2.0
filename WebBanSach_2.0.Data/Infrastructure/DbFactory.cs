using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        WebBanSach_2_0DbContext Init();
    }
    public class DbFactory : Disposable, IDbFactory
    {
        private WebBanSach_2_0DbContext _dbContext;

        public WebBanSach_2_0DbContext Init()
        {
            return _dbContext ?? (_dbContext = new WebBanSach_2_0DbContext());
        }

        protected override void DisposeCore()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }
    }
}
