using System;
using System.Collections.Generic;

namespace WebBanVaLi.Models;

public partial class TUser
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte? LoaiUser { get; set; }

    public virtual ICollection<TGioHang> TGioHangs { get; set; } = new List<TGioHang>();
}
