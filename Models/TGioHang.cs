using System;
using System.Collections.Generic;

namespace WebBanVaLi.Models;

public partial class TGioHang
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public decimal DonGia { get; set; }

    public int SoLuong { get; set; }

    public virtual TDanhMucSp MaSpNavigation { get; set; } = null!;

    public virtual TUser User { get; set; } = null!;
}
