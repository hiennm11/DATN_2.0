using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanSach_2_0.Web.Models
{
    public class CartItem
    {
        public ProductVM Product { get; set; }
        public int Quantity { get; set; }
    }
}