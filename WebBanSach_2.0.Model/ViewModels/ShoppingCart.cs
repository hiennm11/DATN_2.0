﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.ViewModels
{
    public class ShoppingCart
    {
        public List<CartItem> Cart { get; set; }
        public string CartPromoCode { get; set; }
    }
}
