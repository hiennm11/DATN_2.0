using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.Enums
{
    public enum OrderStatus
    {
        AwaitingPayment,
        Cancelled,
        Shipping,       
        Declined,
        Completed,
        Refunded
    }
}
