using System;
using System.Collections.Generic;
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
                                + "<strong>Success!</strong> " + message
                                + "<button type=\"button\" class=\"close\" data-dismiss=\"alert\">&times;</button>"
                                + "</div>"; break;
                default: htmlMessage = "";break;
            }

            return htmlMessage;
        }
    }
}