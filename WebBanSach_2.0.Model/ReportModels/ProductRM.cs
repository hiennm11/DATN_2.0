using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.ReportModels
{
    public class ProductRM
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public double ImportAvgPrice { get; set; }
        public int ImportQuantity { get; set; }
        public double ImportTotalPrice { get; set; }
        public int ExportQuantity { get; set; }
        public double ExportPrice { get; set; }
        public double ExportTotalPrice { get; set; }

    }
}
