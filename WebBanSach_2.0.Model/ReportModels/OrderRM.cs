using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.Enums;

namespace WebBanSach_2_0.Model.ReportModels
{
    public class OrderRM
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
        public string ShipperName { get; set; }
        public string ShipperMobile { get; set; }
        public string DiscountName { get; set; }
        public string DiscountCode { get; set; }
        public double DiscountValue { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public Shipper Shipper { get; set; }
        public Discount Discount { get; set; }
        public ApplicationUser User { get; set; }
    }
}
