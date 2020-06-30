using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Model.ResponseModels
{
    public class ClientOrderDetailResponse
    {
        public List<CartItem> Carts { get; set; }
        public double TotalPrice { get; set; }
        public double BonusPrice { get; set; }

        public ClientOrderDetailResponse()
        {

        }

        public ClientOrderDetailResponse(List<OrderDetailVM> list, OrderVM order)
        {
            Carts = new List<CartItem>();
            foreach (var item in list)
            {
                CartItem cartItem = new CartItem { Product = item.Product, Quantity = item.Quantity };
                Carts.Add(cartItem);
            }
            TotalPrice = Carts.Sum(m => m.Product.Price * (100 - m.Product.Discount.DiscountValue) / 100 * m.Quantity);
            BonusPrice = order.Discount != null ? TotalPrice - (TotalPrice * (100 - order.Discount.DiscountValue) / 100) : 0;
        }
    }
}
