using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Models
{
    public class DonHangNhap: AbstractProps
    {
        [Key]
        public int MaDonHang { get; set; }
        [DisplayName("Ngày nhập")]
        public DateTime NgayNhap { get; set; }
        [DisplayName("Tổng tiền")]
        public double TongTien { get; set; }

        public virtual ICollection<ChiTietDonNhap> ChiTietDonNhaps { get; set; }
    }
}