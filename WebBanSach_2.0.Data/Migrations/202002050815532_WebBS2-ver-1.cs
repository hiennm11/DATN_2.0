namespace WebBanSach_2_0.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBS2ver1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BinhLuans",
                c => new
                    {
                        MaBinhLuan = c.Int(nullable: false, identity: true),
                        MaKhachHang = c.String(maxLength: 128),
                        MaSach = c.Int(nullable: false),
                        NoiDung = c.String(),
                        SoSao = c.Int(nullable: false),
                        NgayBinhLuan = c.DateTime(nullable: false),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MaBinhLuan)
                .ForeignKey("dbo.KhachHangs", t => t.MaKhachHang)
                .ForeignKey("dbo.Saches", t => t.MaSach, cascadeDelete: true)
                .Index(t => t.MaKhachHang)
                .Index(t => t.MaSach);
            
            CreateTable(
                "dbo.KhachHangs",
                c => new
                    {
                        MaKhachHang = c.String(nullable: false, maxLength: 128),
                        Password = c.String(nullable: false),
                        HoTen = c.String(nullable: false),
                        Email = c.String(),
                        DiaChi = c.String(nullable: false),
                        SDT = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MaKhachHang);
            
            CreateTable(
                "dbo.Saches",
                c => new
                    {
                        MaSach = c.Int(nullable: false, identity: true),
                        TenSach = c.String(nullable: false),
                        TenSachKD = c.String(),
                        MaLoai = c.Int(nullable: false),
                        MaNXB = c.Int(nullable: false),
                        MaNCC = c.Int(nullable: false),
                        MoTa = c.String(),
                        Hinh = c.String(),
                        SoTrang = c.String(),
                        GiaBan = c.Double(nullable: false),
                        SoLuong = c.Int(nullable: false),
                        NgayTao = c.DateTime(nullable: false),
                        NgaySua = c.DateTime(nullable: false),
                        LuotMua = c.Int(nullable: false),
                        File = c.String(),
                        SoSao = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.MaSach)
                .ForeignKey("dbo.LoaiSaches", t => t.MaLoai, cascadeDelete: true)
                .ForeignKey("dbo.NhaCungCaps", t => t.MaNCC, cascadeDelete: true)
                .ForeignKey("dbo.NhaXuatBans", t => t.MaNXB, cascadeDelete: true)
                .Index(t => t.MaLoai)
                .Index(t => t.MaNXB)
                .Index(t => t.MaNCC);
            
            CreateTable(
                "dbo.LoaiSaches",
                c => new
                    {
                        MaLoai = c.Int(nullable: false, identity: true),
                        TenLoai = c.String(nullable: false),
                        MoTa = c.String(),
                    })
                .PrimaryKey(t => t.MaLoai);
            
            CreateTable(
                "dbo.NhaCungCaps",
                c => new
                    {
                        MaNCC = c.Int(nullable: false, identity: true),
                        TenNCC = c.String(nullable: false),
                        MoTa = c.String(),
                    })
                .PrimaryKey(t => t.MaNCC);
            
            CreateTable(
                "dbo.NhaXuatBans",
                c => new
                    {
                        MaNXB = c.Int(nullable: false, identity: true),
                        TenNXB = c.String(nullable: false),
                        MoTa = c.String(),
                    })
                .PrimaryKey(t => t.MaNXB);
            
            CreateTable(
                "dbo.ChiTietDonBans",
                c => new
                    {
                        MaDonHang = c.Int(nullable: false),
                        MaSach = c.Int(nullable: false),
                        SoLuong = c.Int(nullable: false),
                        DonGia = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.MaDonHang, t.MaSach })
                .ForeignKey("dbo.DonHangBans", t => t.MaDonHang, cascadeDelete: true)
                .ForeignKey("dbo.Saches", t => t.MaSach, cascadeDelete: true)
                .Index(t => t.MaDonHang)
                .Index(t => t.MaSach);
            
            CreateTable(
                "dbo.DonHangBans",
                c => new
                    {
                        MaDonHang = c.Int(nullable: false, identity: true),
                        MaKhachHang = c.String(maxLength: 128),
                        HoTenKH = c.String(),
                        DiaChiKH = c.String(),
                        EmailKH = c.String(),
                        SdtKH = c.String(),
                        NgayBan = c.DateTime(nullable: false),
                        TongTien = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.MaDonHang)
                .ForeignKey("dbo.KhachHangs", t => t.MaKhachHang)
                .Index(t => t.MaKhachHang);
            
            CreateTable(
                "dbo.ChiTietDonNhaps",
                c => new
                    {
                        MaDonHang = c.Int(nullable: false),
                        MaSach = c.Int(nullable: false),
                        SoLuong = c.Int(nullable: false),
                        DonGia = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.MaDonHang, t.MaSach })
                .ForeignKey("dbo.DonHangNhaps", t => t.MaDonHang, cascadeDelete: true)
                .ForeignKey("dbo.Saches", t => t.MaSach, cascadeDelete: true)
                .Index(t => t.MaDonHang)
                .Index(t => t.MaSach);
            
            CreateTable(
                "dbo.DonHangNhaps",
                c => new
                    {
                        MaDonHang = c.Int(nullable: false, identity: true),
                        NgayNhap = c.DateTime(nullable: false),
                        TongTien = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.MaDonHang);
            
            CreateTable(
                "dbo.ChucVus",
                c => new
                    {
                        MaChucVu = c.Int(nullable: false, identity: true),
                        TenChucVu = c.String(),
                        MoTa = c.String(),
                    })
                .PrimaryKey(t => t.MaChucVu);
            
            CreateTable(
                "dbo.NhanViens",
                c => new
                    {
                        MaNV = c.String(nullable: false, maxLength: 128),
                        Password = c.String(nullable: false),
                        HoTen = c.String(nullable: false),
                        Email = c.String(),
                        DiaChi = c.String(nullable: false),
                        SDT = c.String(nullable: false),
                        MaChucVu = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MaNV)
                .ForeignKey("dbo.ChucVus", t => t.MaChucVu, cascadeDelete: true)
                .Index(t => t.MaChucVu);
            
            CreateTable(
                "dbo.TacGias",
                c => new
                    {
                        MaTacGia = c.Int(nullable: false, identity: true),
                        TenTacGia = c.String(nullable: false),
                        ThongTin = c.String(),
                    })
                .PrimaryKey(t => t.MaTacGia);
            
            CreateTable(
                "dbo.ThamGias",
                c => new
                    {
                        MaSach = c.Int(nullable: false),
                        MaTacGia = c.Int(nullable: false),
                        VaiTro = c.String(),
                    })
                .PrimaryKey(t => new { t.MaSach, t.MaTacGia })
                .ForeignKey("dbo.Saches", t => t.MaSach, cascadeDelete: true)
                .ForeignKey("dbo.TacGias", t => t.MaTacGia, cascadeDelete: true)
                .Index(t => t.MaSach)
                .Index(t => t.MaTacGia);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ThamGias", "MaTacGia", "dbo.TacGias");
            DropForeignKey("dbo.ThamGias", "MaSach", "dbo.Saches");
            DropForeignKey("dbo.NhanViens", "MaChucVu", "dbo.ChucVus");
            DropForeignKey("dbo.ChiTietDonNhaps", "MaSach", "dbo.Saches");
            DropForeignKey("dbo.ChiTietDonNhaps", "MaDonHang", "dbo.DonHangNhaps");
            DropForeignKey("dbo.ChiTietDonBans", "MaSach", "dbo.Saches");
            DropForeignKey("dbo.DonHangBans", "MaKhachHang", "dbo.KhachHangs");
            DropForeignKey("dbo.ChiTietDonBans", "MaDonHang", "dbo.DonHangBans");
            DropForeignKey("dbo.Saches", "MaNXB", "dbo.NhaXuatBans");
            DropForeignKey("dbo.Saches", "MaNCC", "dbo.NhaCungCaps");
            DropForeignKey("dbo.Saches", "MaLoai", "dbo.LoaiSaches");
            DropForeignKey("dbo.BinhLuans", "MaSach", "dbo.Saches");
            DropForeignKey("dbo.BinhLuans", "MaKhachHang", "dbo.KhachHangs");
            DropIndex("dbo.ThamGias", new[] { "MaTacGia" });
            DropIndex("dbo.ThamGias", new[] { "MaSach" });
            DropIndex("dbo.NhanViens", new[] { "MaChucVu" });
            DropIndex("dbo.ChiTietDonNhaps", new[] { "MaSach" });
            DropIndex("dbo.ChiTietDonNhaps", new[] { "MaDonHang" });
            DropIndex("dbo.DonHangBans", new[] { "MaKhachHang" });
            DropIndex("dbo.ChiTietDonBans", new[] { "MaSach" });
            DropIndex("dbo.ChiTietDonBans", new[] { "MaDonHang" });
            DropIndex("dbo.Saches", new[] { "MaNCC" });
            DropIndex("dbo.Saches", new[] { "MaNXB" });
            DropIndex("dbo.Saches", new[] { "MaLoai" });
            DropIndex("dbo.BinhLuans", new[] { "MaSach" });
            DropIndex("dbo.BinhLuans", new[] { "MaKhachHang" });
            DropTable("dbo.ThamGias");
            DropTable("dbo.TacGias");
            DropTable("dbo.NhanViens");
            DropTable("dbo.ChucVus");
            DropTable("dbo.DonHangNhaps");
            DropTable("dbo.ChiTietDonNhaps");
            DropTable("dbo.DonHangBans");
            DropTable("dbo.ChiTietDonBans");
            DropTable("dbo.NhaXuatBans");
            DropTable("dbo.NhaCungCaps");
            DropTable("dbo.LoaiSaches");
            DropTable("dbo.Saches");
            DropTable("dbo.KhachHangs");
            DropTable("dbo.BinhLuans");
        }
    }
}
