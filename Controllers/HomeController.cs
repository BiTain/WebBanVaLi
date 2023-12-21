using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebBanVaLi.Models;
using WebBanVaLi.Models.Authentication;
using WebBanVaLi.ViewModels;
using X.PagedList;

namespace WebBanVaLi.Controllers
{
    public class HomeController : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authentication]
        public IActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x=>x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham,pageNumber,pageSize);

            return View(lst);
        }

        public IActionResult GioHang()
        {
            var user = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(user))
            {
                return Redirect("/Access/Login");
            }
            var items = db.TGioHangs.Include(c => c.MaSpNavigation).Where(c => c.UserId.Equals(user));
            return View(items.ToList());
        }

        public IActionResult SanPhamTheoLoai(String maloai,int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().Where(x=>x.MaLoai==maloai).
                OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            ViewBag.maloai=maloai;
            return View(lst);
        }

        public IActionResult ChiTietSanPham(String maSp)
        {
            var sanPham = db.TDanhMucSps.SingleOrDefault(x=>x.MaSp==maSp);
            var anhSanPham=db.TAnhSps.Where(x=>x.MaSp==maSp).ToList();
            ViewBag.anhSanPham=anhSanPham;
            return View(sanPham);
        }

        public IActionResult ProductDetail(string maSp)
        {
            var sanPham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
            var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
            var homeProductDetailViewModel = new HomeProductDetailViewModel 
            { danhMucSp = sanPham,anhSps = anhSanPham };
            return View(homeProductDetailViewModel);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}