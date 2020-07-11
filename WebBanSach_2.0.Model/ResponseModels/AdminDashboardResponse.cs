using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Model.ResponseModels
{
    public class AdminDashboardResponse
    {
        public AdminChartResponse Charts { get; set; }
        public List<ProductRankVM> ProductRanks { get; set; }
        public double MonthlyEarnings { get; set; }
        public double AnnualEarnings { get; set; }
        public double OrderPercentage { get; set; }
        public double WaitingOrderPercentage { get; set; }
        public double AcceptedOrderPercentage { get; set; }
        public double InProgressOrderPercentage { get; set; }
        public double ShippingOrderPercentage { get; set; }
        public double DeliveriedOrderPercentage { get; set; }
        public double CompletedOrderPercentage { get; set; }
        public int WaitingOrderCount { get; set; }
    }
}
