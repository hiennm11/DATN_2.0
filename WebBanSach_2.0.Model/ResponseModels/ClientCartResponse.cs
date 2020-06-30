using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Model.ResponseModels
{
    public class ClientCartResponse
    {
        public List<CartItem> Cart { get; set; }
        public double TotalPrice { get; set; }
        public OrderVM OrderInfo { get; set; }
        public string PromoCode { get; set; }
        public double CodePriceBonus { get; set; }
    }
}
