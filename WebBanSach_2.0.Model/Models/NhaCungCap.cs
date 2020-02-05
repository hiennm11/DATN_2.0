using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Models
{
    public class NhaCungCap: AbstractProps
    {
        [Key]
        [DisplayName("Nhà cung cấp")]
        public int MaNCC { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên nhà cung cấp")]
        [DisplayName("Nhà cung cấp")]
        public string TenNCC { get; set; }
        [DisplayName("Mô tả")]
        public string MoTa { get; set; }
    }
}