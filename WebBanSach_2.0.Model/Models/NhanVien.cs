using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Models
{
    public class NhanVien: AbstractProps
    {
        [Key]
        [Required(ErrorMessage = "Bạn chưa nhập username")]
        [DisplayName("User name")]
        public string MaNV { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập họ tên")]
        [DisplayName("Họ tên")]
        public string HoTen { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ")]
        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại")]
        [DisplayName("Số điện thoại")]
        public string SDT { get; set; }

        public int MaChucVu { get; set; }
        [DisplayName("Trạng thái")]

        public virtual ChucVu ChucVuNhanVien { get; set; }
    }
}