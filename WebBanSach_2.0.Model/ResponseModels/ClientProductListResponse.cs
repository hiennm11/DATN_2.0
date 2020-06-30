using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Model.ResponseModels
{
    public class ClientProductListResponse
    {
        public Pagination.IndexViewModel<ProductVM> ProductList { get; set; }
        public IEnumerable<ProductVM> HotProducts { get; set; }
        public IEnumerable<ProductVM> NewsProducts { get; set; }

    }
}
