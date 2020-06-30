using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.ViewModels
{
    public class ShipperVM
    {
        public int ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string ShipperMobile { get; set; }

        public ICollection<OrderVM> Orders { get; set; }
    }
}
