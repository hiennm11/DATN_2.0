using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.Enums
{
    public enum DiscountType
    {
        [Display(Name = "Tất cả")]
        All,
        [Display(Name = "Sản phẩm")]
        Product,
        [Display(Name = "Hóa đơn")]
        Order, 
        [Display(Name = "Tài khoản")]
        Account
    }
}
