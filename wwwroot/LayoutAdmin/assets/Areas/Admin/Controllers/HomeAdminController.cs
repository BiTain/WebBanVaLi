using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WebBanVaLi.Models;
using WebBanVaLi.Models.Authentication;
using X.PagedList;

namespace WebBanVaLi.Areas.Admin.Controllers
{

    [Authentication]
/*    [AuthenticationRedirect]*/
    [Area("admin")] // tên của Area thôi
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        QlbanVaLiContext db=new QlbanVaLiContext();
        [Route("")]
        [Route("index")]
        public IActionResult index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var list = db.TDanhMucSps.AsNoTracking().OrderBy(i => i.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(list, pageNumber, pageSize);
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
        public IActionResult ThemSanPhamMoi(TDanhMucSp sanpham)
        {
            if (ModelState.IsValid)
            {
                db.TDanhMucSps.Add(sanpham);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham");
            }
            return View(sanpham);
        }


        [Route("SuaSanPham")]
        [HttpGet]
        public IActionResult SuaSanPham(string? masp)
        {
            // lấy sản phẩm cần chỉnh sửa
            var sp = db.TDanhMucSps.Find(masp);
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            return View(sp);
        }

        [Route("SuaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(TDanhMucSp sanpham)
        {
            if (ModelState.IsValid)
            {
                db.Update(sanpham);
                // dùng 1 cái này cũng được
/*                db.Entry(sanpham).State = EntityState.Modified;*/
                db.SaveChanges();
                return RedirectToAction("Index", "HomeAdmin");
            }
            return View(sanpham);
        }

        [Route("XoaSanPham")]
        [HttpGet]
        public IActionResult XoaSanPham(string? masp)
        {
            TempData["tb"] = "";
            var chitietsp = db.TChiTietSanPhams.Where(i => i.MaSp == masp).ToList();
            if (chitietsp.Any())
            {
                TempData["tb"] = "Không xóa được sản phẩm này";
                return RedirectToAction("Index", "HomeAdmin");
            }
            var anhsp=db.TAnhSps.Where(i => i.MaSp== masp).ToList();    
            if (anhsp.Any())
                db.RemoveRange(anhsp);
            db.Remove(db.TDanhMucSps.Find(masp));
            db.SaveChanges();
            TempData["tb"] = "Xóa sản phẩm thành công";
            return RedirectToAction("Index", "HomeAdmin");
        }
    }
}
