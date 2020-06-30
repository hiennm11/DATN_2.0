using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.Enums
{
    public enum PaymentMethod
    {
        [Display(Name = "Chuyển khoản ngân hàng")]
        ByBankTransferInAdvance,
        [Display(Name = "Hóa đơn")]
        ByInvoice,
        [Display(Name = "Tiền mặt")]
        Cash,
        [Display(Name = "Nhận hàng trả tiền")]
        COD
    }
}
