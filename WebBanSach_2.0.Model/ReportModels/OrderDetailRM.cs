using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.ReportModels
{
    public class OrderDetailRM
    {
        public string ProductName { get; set; }
        public string AuthorName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double DiscountValue { get; set; }
    }
}
