using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Service.Interfaces
{
    public interface IClientCategoryService
    {
        Task<IEnumerable<CategoryVM>> GetAllCategory(); 
    }
}
