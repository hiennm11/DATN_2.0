using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Model.ResponseModels
{
    public class AdminListProductResponse
    {
        public Pagination.IndexViewModel<ProductVM> Products { get; set; }
        public IEnumerable<CategoryVM> Categories { get; set; }
    }
}
