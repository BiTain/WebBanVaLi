using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanVaLi.Models;
using X.PagedList;
using System.Web;
using WebBanVaLi.Areas.Admin.Models;
using WebBanVaLi.Models.Authentication;

namespace WebBanVaLi.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    [Authentication]
    public class HomeAdminController : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        [Route("")]
        [Route("index")]

        public IActionResult Index(int? page, string? tensp)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst;
            if (tensp != null)
            {
                ViewBag.tensp = tensp;
                List<TDanhMucSp> sps = db.TDanhMucSps.Where(s => s.TenSp.Contains(tensp)).ToList();
                lst = new PagedList<TDanhMucSp>(sps, pageNumber, pageSize);
                return View(lst);
            }
            lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }

        [Route("ThemSanPhamMoi")]
        [HttpGet]
        public IActionResult ThemSanPhamMoi()
        {           
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(),"MaChatLieu","ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            return View();
        }

        [Route("ThemSanPhamMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemSanPhamMoiAsync(Sanpham sanpham)
        {           
            if (ModelState.IsValid)
            {
                string AnhDaiDien = "";
                if (sanpham.AnhDaiDien != null && sanpham.AnhDaiDien.Length > 0)
                {
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductsImages/Images");
                    var fileName = Path.GetRandomFileName() + Path.GetExtension(sanpham.AnhDaiDien.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                        await sanpham.AnhDaiDien.CopyToAsync(fileStream);
                    AnhDaiDien = fileName;
                }
                db.TDanhMucSps.Add(new TDanhMucSp(sanpham.MaSp, sanpham.TenSp, sanpham.MaChatLieu, sanpham.Model, sanpham.CanNang, sanpham.DoNoi, sanpham.MaHangSx, sanpham.MaNuocSx, sanpham.ThoiGianBaoHanh, sanpham.MaDt, AnhDaiDien, sanpham.GiaLonNhat, sanpham.GioiThieuSp));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sanpham);
        }
        public static string url = "";

        [Route("SuaSanPham")]
        [HttpGet]
        public IActionResult SuaSanPham(string maSp)
        {           
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            var sanPham = db.TDanhMucSps.Find(maSp);
            url = sanPham.AnhDaiDien;
            return View(sanPham);
        }

        [Route("SuaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(TDanhMucSp sanPham)
        {            
            if (ModelState.IsValid)
            {
                if (sanPham.AnhDaiDien == null)
                    sanPham.AnhDaiDien = url;
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","HomeAdmin");
            }
            return View(sanPham);
        }

        [Route("XoaSanPham")]
        [HttpGet]
        public IActionResult XoaSanPham(string masp)
        {            
            TempData["Message"] = "";
            var chiTietSanPhams=db.TChiTietSanPhams.Where(x=>x.MaSp== masp).ToList();
            if(chiTietSanPhams.Count()>0)
            {
                TempData["Message"] = "Không xóa được sản phẩm này";
                return RedirectToAction("Index", "HomeAdmin");
            }
            var anhSanPhams = db.TAnhSps.Where(x=>x.MaSp== masp);
            if (anhSanPhams.Any()) 
                db.RemoveRange(anhSanPhams);
            db.Remove(db.TDanhMucSps.Find(masp));
            db.SaveChanges();
            TempData["Message"] = "Sản phẩm đã được xóa";
            return RedirectToAction("Index", "HomeAdmin");
        }
    }
}
