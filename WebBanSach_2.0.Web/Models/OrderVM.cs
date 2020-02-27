using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanSach_2_0.Web.Models
{
    public class OrderVM
    {
        public int ID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string PaymentMethod { get; set; }
        public bool PaymentStatus { get; set; }
        public int Status { get; set; }
    }

    public class OrderDetailVM
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}