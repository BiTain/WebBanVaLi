using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanVaLi.Models;
using WebBanVaLi.Models.ProductModels;

namespace WebBanVaLi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            var sanPham = (from p in db.TDanhMucSps select new Product
            {
                MaSp = p.MaSp,
                TenSp = p.TenSp,
                MaLoai = p.MaLoai,
                AnhDaiDien = p.AnhDaiDien,
                GiaLonNhat = p.GiaLonNhat
            }).ToList();
            return sanPham;
        }

        [HttpGet("{maloai}")]
        public IEnumerable<Product> GetProductsByCategory(string maLoai)
        {
            var sanPham = (from p in db.TDanhMucSps
                           where p.MaLoai == maLoai
                           select new Product
                           {
                               MaSp = p.MaSp,
                               TenSp = p.TenSp,
                               MaLoai = p.MaLoai,
                               AnhDaiDien = p.AnhDaiDien,
                               GiaLonNhat = p.GiaLonNhat
                           }).ToList();
            return sanPham;
        }

        [HttpGet]
        public IActionResult GetCartItems()
        {
            //Request.HttpContext.Session.SetString("UserName", "a");
            var user = Request.HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(user))
            {
                return BadRequest("Chưa đăng nhập");
            }

            var items = db.TGioHangs.Include(c => c.MaSpNavigation).Where(c => c.UserId.Equals(user)).ToList();
            return Ok(items);
        }

        [HttpGet]
        public IActionResult UpdateCartItem(string maSP, int soLuong = 1)
        {
            //Request.HttpContext.Session.SetString("UserName", "a");
            var user = Request.HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(user))
            {
                return BadRequest("Chưa đăng nhập");
            }

            var check = db.TDanhMucSps.FirstOrDefault(s => s.MaSp.Equals(maSP));
            if (check == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }

            var checkItem = db.TGioHangs.FirstOrDefault(c => c.UserId.Equals(user) && c.MaSp.Equals(maSP));
            if (checkItem == null)
            {
                db.TGioHangs.Add(new TGioHang
                {
                    DonGia = check.GiaNhoNhat == null ? 0 : (decimal)check.GiaNhoNhat,
                    MaSp = check.MaSp,
                    SoLuong = soLuong,
                    UserId = user
                });
            }
            else
            {
                if (soLuong <= 0)
                {
                    db.TGioHangs.Remove(checkItem);
                }
                else
                {
                    checkItem.SoLuong = soLuong;
                }
            }
            var updateResult = db.SaveChanges();

            return Ok(updateResult);
        }
    }


}
