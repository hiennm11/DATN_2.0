using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Model.ResponseModels
{
    public class AdminProductDetailResponse
    {
        public ProductVM Product { get; set; }
        public IEnumerable<CategoryVM> Categories { get; set; }
    }
}
