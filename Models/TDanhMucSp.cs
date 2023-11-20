using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace WebBanVaLi.Models;

public partial class TDanhMucSp
{

    public TDanhMucSp() { }
    public TDanhMucSp(string maSp, string? tenSp, string? maChatLieu, string? model, double? canNang, double? doNoi, string? maHangSx, string? maNuocSx, double? thoiGianBaoHanh, string? maDt, string anhDaiDien, decimal? giaLonNhat, string? GioiThieuSp)
    {
        MaSp = maSp;
        TenSp = tenSp;
        MaChatLieu = maChatLieu;
        Model = model;
        CanNang = canNang;
        DoNoi = doNoi;
        MaHangSx = maHangSx;
        MaNuocSx = maNuocSx;
        ThoiGianBaoHanh = thoiGianBaoHanh;
        MaDt = maDt;
        AnhDaiDien = anhDaiDien;
        GiaLonNhat = giaLonNhat;
        this.GioiThieuSp = GioiThieuSp;
    }

    public string MaSp { get; set; } = null!;

    public string? TenSp { get; set; }

    public string? MaChatLieu { get; set; }

    public string? NganLapTop { get; set; }

    public string? Model { get; set; }

    public double? CanNang { get; set; }

    public double? DoNoi { get; set; }

    public string? MaHangSx { get; set; }

    public string? MaNuocSx { get; set; }

    public string? MaDacTinh { get; set; }

    public string? Website { get; set; }

    public double? ThoiGianBaoHanh { get; set; }

    public string? GioiThieuSp { get; set; }

    public double? ChietKhau { get; set; }

    public string? MaLoai { get; set; }

    public string? MaDt { get; set; }

    public string? AnhDaiDien { get; set; }

    public decimal? GiaNhoNhat { get; set; }

    public decimal? GiaLonNhat { get; set; }

    public virtual TChatLieu? MaChatLieuNavigation { get; set; }

    public virtual TLoaiDt? MaDtNavigation { get; set; }

    public virtual THangSx? MaHangSxNavigation { get; set; }

    public virtual TLoaiSp? MaLoaiNavigation { get; set; }

    public virtual TQuocGium? MaNuocSxNavigation { get; set; }

    public virtual ICollection<TAnhSp> TAnhSps { get; set; } = new List<TAnhSp>();

    public virtual ICollection<TChiTietSanPham> TChiTietSanPhams { get; set; } = new List<TChiTietSanPham>();
}
