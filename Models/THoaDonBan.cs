using System;
using System.Collections.Generic;

namespace WebBanVaLi.Models;

public partial class THoaDonBan
{
    public int MaHoaDon { get; set; }

    public DateTime? NgayHoaDon { get; set; }

    public string username { get; set; }

    public decimal? TongTienHd { get; set; }

    public double? GiamGiaHd { get; set; }

    public string? PhuongThucThanhToan { get; set; }

    public string? MaSoThue { get; set; }

    public string? ThongTinThue { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<TChiTietHdb> TChiTietHdbs { get; set; } = new List<TChiTietHdb>();

    public THoaDonBan() { }
    public THoaDonBan(int maHoaDon, DateTime? ngayHoaDon, string? maKhachHang, string? maNhanVien, decimal? tongTienHd, string? phuongThucThanhToan, string ghiChu)
    {
        MaHoaDon = maHoaDon;
        NgayHoaDon = ngayHoaDon;
        username = maKhachHang;
        TongTienHd = tongTienHd;
        PhuongThucThanhToan = phuongThucThanhToan;
        GhiChu = ghiChu;
    }
}
