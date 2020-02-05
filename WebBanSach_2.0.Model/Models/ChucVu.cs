using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanSach_2_0.Models
{
    public class ChucVu
    {
        [Key]
        public int MaChucVu { get; set; }
        [DisplayName("Tên chức vụ")]
        public string TenChucVu { get; set; }
        [DisplayName("Mô tả")]
        public string MoTa { get; set; }        
    }
}