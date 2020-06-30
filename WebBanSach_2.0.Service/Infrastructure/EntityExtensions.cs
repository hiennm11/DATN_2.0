using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.Enums;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Service.Infrastructure
{
    public static class EntityExtensions
    {
        public static string RegularExpressionFilter = @"[^`|\~|\!|\@|\#|\$|\%|\^|\&|*|(|)|\+|\=|[|\{|]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\;|\:]+";
        public static string ConvertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace(" ","-").ToLower();
        }
        
        public static string HtmlStatusMessage(StatusMessageId? status)
        {
            string message;
            string htmlMessage;            
           
            switch (status)
            {
                case StatusMessageId.AddSuccess:
                    message = "Đã tạo thành công bản ghi.";
                    htmlMessage = "<div class=\"alert alert-success alert-dismissible fade show\">"
                                + "<strong>Success!</strong> " + message
                                + "<button type=\"button\" class=\"close\" data-dismiss=\"alert\">&times;</button>"
                                + "</div>"; break;
                case StatusMessageId.UpdateSuccess:
                    message = "Đã cập nhật thành công bản ghi.";
                    htmlMessage = "<div class=\"alert alert-primary alert-dismissible fade show\">"
                                + "<strong>Success!</strong> " + message
                                + "<button type=\"button\" class=\"close\" data-dismiss=\"alert\">&times;</button>"
                                + "</div>"; break;
                case StatusMessageId.DeleteSuccess:
                    message = "Đã xóa thành công bản ghi.";
                    htmlMessage = "<div class=\"alert alert-warning alert-dismissible fade show\">"
                                + "<strong>Success!</strong> " + message
                                + "<button type=\"button\" class=\"close\" data-dismiss=\"alert\">&times;</button>"
                                + "</div>"; break;
                case StatusMessageId.Error:
                    message = "Đã có lỗi xảy ra.";
                    htmlMessage = "<div class=\"alert alert-danger alert-dismissible fade show\">"
                                + "<strong>Error!</strong> " + message
                                + "<button type=\"button\" class=\"close\" data-dismiss=\"alert\">&times;</button>"
                                + "</div>"; break;
                default: htmlMessage = "";break;
            }

            return htmlMessage;
        }

        public static List<DateTime> GetWeekDate(DateTime now)
        {
            DateTime reference = DateTime.Now;
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;

            int weekOfYear = calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var firstDate = FirstDateOfWeek(reference.Year, weekOfYear);
            var week = Enumerable.Range(0, 7).Select(d => firstDate.AddDays(d)).ToList();

            return week;
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = Convert.ToInt32(CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) - Convert.ToInt32(jan1.DayOfWeek);
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            System.Globalization.CultureInfo curCulture = CultureInfo.CurrentCulture;
            int firstWeek = curCulture.Calendar.GetWeekOfYear(jan1, curCulture.DateTimeFormat.CalendarWeekRule, curCulture.DateTimeFormat.FirstDayOfWeek);
            if (firstWeek <= 1)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }

    }
}