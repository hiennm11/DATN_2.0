using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Models;

namespace WebBanSach_2_0.Data
{
    public class WebBanSach_2_0DbContext : DbContext
    {
        public WebBanSach_2_0DbContext() : base("WebBanSach_2_0")
        {
        }

        public WebBanSach_2_0DbContext Create()
        {
            return new WebBanSach_2_0DbContext();
        }

        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<Sach> Saches { get; set; }
        public DbSet<TacGia> TacGias { get; set; }
        public DbSet<LoaiSach> LoaiSaches { get; set; }
        public DbSet<NhaXuatBan> NhaXuatBans { get; set; }
        public DbSet<BinhLuan> BinhLuans { get; set; }
        public DbSet<ThamGia> ThamGias { get; set; }
        public DbSet<DonHangBan> DonHangBans { get; set; }
        public DbSet<ChiTietDonBan> ChiTietDonBans { get; set; }
        public DbSet<DonHangNhap> DonHangNhaps { get; set; }
        public DbSet<ChiTietDonNhap> ChiTietDonNhaps { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<ChucVu> ChucVus { get; set; }

    }
}
