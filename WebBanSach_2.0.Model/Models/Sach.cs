using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Models
{
    public class Sach: AbstractProps
    {
        [Key]
        public int MaSach { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên sách")]
        [DisplayName("Tựa sách")]
        public string TenSach { get; set; }
        public string TenSachKD { get; set; }
        [DisplayName("Danh mục")]
        public int MaLoai { get; set; }
        [DisplayName("Nhà xuất bản")]
        public int MaNXB { get; set; }
        [DisplayName("Nhà cung cấp")]
        public int MaNCC { get; set; }
        [DisplayName("Mô tả")]
        public string MoTa { get; set; }
        [DisplayName("Hình")]
        public string Hinh { get; set; }
        [DisplayName("Số trang")]
        public string SoTrang { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập giá")]
        [DisplayName("Giá")]
        public double GiaBan { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập số lượng")]
        [DisplayName("Số lượng")]
        public int SoLuong { get; set; }
        [DisplayName("Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime NgayTao { get; set; }
        [DisplayName("Ngày sửa")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime NgaySua { get; set; }
        public int LuotMua { get; set; }
        public string File { get; set; }
        public double SoSao { get; set; }

        

       
        public virtual LoaiSach Loai { get; set; }
        public virtual NhaXuatBan NhaXuatBan { get; set; }
        public virtual NhaCungCap NhaCungCap { get; set; }
        public virtual ICollection<BinhLuan> BinhLuans { get; set; }

    }
}