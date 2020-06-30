using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.ResponseModels
{
    public class AdminChartResponse
    {
        public List<AreaChartValue> AreaCharts { get; set; }
    }

    public class AreaChartValue
    {
        public DayOfWeek DayOfWeek { get; set; }
        public DateTime Date { get; set; }
        public double DateEarning { get; set; }
    }
}
