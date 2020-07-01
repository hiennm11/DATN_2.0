using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.ResponseModels
{
    public class AdminChartResponse
    {
        public AreaChart AreaChart { get; set; }
        public BarChart BarChart { get; set; }
        public PieChart PieChart { get; set; }
    }

    public class AreaChart
    {
        public List<string> Date { get; set; }
        public List<double> DateEarning { get; set; }
    }

    public class BarChart
    {
        public List<string> Date { get; set; }
        public List<double> DateEarning { get; set; }
    }

    public class PieChart
    {
        public List<string> Roles { get; set; }
        public List<double> Percentage { get; set; }
    }
}
