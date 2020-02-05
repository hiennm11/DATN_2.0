using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Models
{
    public class NhaXuatBan: AbstractProps
    {
        [Key]
        [DisplayName("Nhà xuất bản")]
        public int MaNXB { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên NXB")]
        [DisplayName("Nhà xuất bản")]
        public string TenNXB { get; set; }
        [DisplayName("Mô tả")]
        public string MoTa { get; set; }

    }
}