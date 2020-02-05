using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanSach_2_0.Models
{
    public class ChiTietDonBan
    {
        [Key]
        [Column(Order = 1)]
        public int MaDonHang { get; set; }
        [Key]
        [Column(Order = 2)]
        [DisplayName("Tên sách")]
        public int MaSach { get; set; }
        [DisplayName("Số lượng")]
        public int SoLuong { get; set; }
        [DisplayName("Đơn giá")]
        public double DonGia { get; set; }

        public virtual Sach Sach { get; set; }
        public virtual DonHangBan DonHangBan { get; set; }
    }
}