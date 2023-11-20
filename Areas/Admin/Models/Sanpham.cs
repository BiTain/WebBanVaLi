using WebBanVaLi.Models;

namespace WebBanVaLi.Areas.Admin.Models
{
    public class Sanpham
    {
        public string MaSp { get; set; } = null!;

        public string? TenSp { get; set; }

        public string? MaChatLieu { get; set; }

        public string? Model { get; set; }

        public double? CanNang { get; set; }

        public double? DoNoi { get; set; }

        public string? MaHangSx { get; set; }

        public string? MaNuocSx { get; set; }

        public double? ThoiGianBaoHanh { get; set; }

        public string? MaDt { get; set; }

        public IFormFile AnhDaiDien { get; set; }

        public decimal? GiaLonNhat { get; set; }

        public string? GioiThieuSp { get; set; }

    }
}

