using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanSach_2_0.Model.Enums;

namespace WebBanSach_2_0.Model.ViewModels
{
    public class OrderVM
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public bool PaymentStatus { get; set; }
        public OrderStatus Status { get; set; }

        public DiscountVM Discount { get; set; }
    }

    public class OrderDetailVM
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }

        public OrderVM Orders { get; set; }
        public ProductVM Product { get; set; }
    }
}