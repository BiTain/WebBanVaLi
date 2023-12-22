using System;
using System.Collections.Generic;

namespace WebBanVaLi.Models;

public partial class TChiTietHdb
{
    public int MaHoaDon { get; set; }

    public string MaSp{ get; set; }

    public int? SoLuongBan { get; set; }

    public decimal? DonGiaBan { get; set; }

    public double? GiamGia { get; set; }

    public string? GhiChu { get; set; }

    public virtual TChiTietSanPham MaChiTietSpNavigation { get; set; } = null!;

    public virtual THoaDonBan MaHoaDonNavigation { get; set; } = null!;
}
