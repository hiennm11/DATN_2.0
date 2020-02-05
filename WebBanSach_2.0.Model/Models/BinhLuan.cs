using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanSach_2_0.Models
{
    public class BinhLuan
    {
        [Key]
        public int MaBinhLuan { get; set; }
        public string MaKhachHang { get; set; }
        [DisplayName("Sách")]
        public int MaSach { get; set; }
        [DisplayName("Nội dung")]
        public string NoiDung { get; set; }

        public int SoSao { get; set; }
        [DisplayName("Ngày bình luận")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime NgayBinhLuan { get; set; }
        [DisplayName("Trạng thái")]
        public bool TrangThai { get; set; }

        public virtual Sach Sach { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }
}