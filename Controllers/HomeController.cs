using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebBanVaLi.Models;
using WebBanVaLi.Services;
using WebBanVaLi.ViewModels;
using X.PagedList;

namespace WebBanVaLi.Controllers
{
    public class HomeController : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        private readonly ILogger<HomeController> _logger;
        private readonly IVnPayService _vnPayService;

        public HomeController(ILogger<HomeController> logger, IVnPayService vnPayService)
        {
            _logger = logger;
            _vnPayService = vnPayService;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.OrderBy(x => x.TenSp).ToList();
            var lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);

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

        public IActionResult SanPhamTheoLoai(String maloai, int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().Where(x => x.MaLoai == maloai).
                OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            ViewBag.maloai = maloai;
            return View(lst);
        }

        public IActionResult ChiTietSanPham(String maSp)
        {
            var sanPham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
            var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
            ViewBag.anhSanPham = anhSanPham;
            return View(sanPham);
        }

        public IActionResult ProductDetail(string maSp)
        {
            var sanPham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
            var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
            var homeProductDetailViewModel = new HomeProductDetailViewModel
            { danhMucSp = sanPham, anhSps = anhSanPham };
            return View(homeProductDetailViewModel);
        }

        public IActionResult Checkout()
        {
            var username = HttpContext.Session.GetString("UserName");

            var userCartItems = db.TGioHangs.Include(c => c.MaSpNavigation).Where(c => c.UserId.Equals(username)).ToList();

            CheckoutViewModel view = new CheckoutViewModel(username, userCartItems);

            return View("Checkout", view);
        }

        [HttpPost]
        public IActionResult PlaceOrder()
        {
            var username = HttpContext.Session.GetString("UserName");
            var userCartItems = db.TGioHangs.Include(c => c.MaSpNavigation).Where(c => c.UserId.Equals(username)).ToList();

            string selectedPaymentMethod = HttpContext.Request.Form["payment"];
            decimal tongTienHoaDon = 0;
            DateTime currentDay = DateTime.Now;

            foreach (var cartItem in userCartItems)
            {
                decimal tongTien = cartItem.DonGia * cartItem.SoLuong;
                tongTienHoaDon += tongTien;
            }

            if (selectedPaymentMethod == "vnpay")
            {
                int tongTienInt = Decimal.ToInt32(decimal.Round(tongTienHoaDon));
                string url = _vnPayService.CreatePaymentUrl(tongTienInt, HttpContext);

                return Redirect(url);
            }
           
            // Tạo danh sách chi tiết hóa đơn
            List<TChiTietHdb> chiTietHDBs = new List<TChiTietHdb>();

            THoaDonBan hoaDonBan = new THoaDonBan
            {
                NgayHoaDon = currentDay,
                TongTienHd = tongTienHoaDon,
                username = username,
                PhuongThucThanhToan = selectedPaymentMethod
            };

            db.THoaDonBans.Add(hoaDonBan);
            db.SaveChanges();

            var foundInvoice = db.THoaDonBans.FirstOrDefault(hd => hd.NgayHoaDon == currentDay && hd.username == username);

            if (foundInvoice != null)
            {
                foreach(var cartItem in userCartItems)
                {

                    TChiTietHdb chiTietHDB = new TChiTietHdb
                    {
                        MaHoaDon = foundInvoice.MaHoaDon,
                        MaSp = cartItem.MaSp,
                        SoLuongBan = cartItem.SoLuong,
                        DonGiaBan = cartItem.DonGia,
                    };

                    chiTietHDBs.Add(chiTietHDB);
                }
            }

            db.TChiTietHdbs.AddRange(chiTietHDBs);
            db.SaveChanges();

            db.TGioHangs.RemoveRange(userCartItems);
            db.SaveChanges();

            ViewBag.PaymentMethod = "Thanh toán khi nhận hàng";

            return View("CheckoutSuccess");
        }

        public IActionResult VNPaySuccess()
        {
            var username = HttpContext.Session.GetString("UserName");
            var userCartItems = db.TGioHangs.Include(c => c.MaSpNavigation).Where(c => c.UserId.Equals(username)).ToList();

            decimal tongTienHoaDon = 0;
            DateTime currentDay = DateTime.Now;

            foreach (var cartItem in userCartItems)
            {
                decimal tongTien = cartItem.DonGia * cartItem.SoLuong;
                tongTienHoaDon += tongTien;
            }

            // Tạo danh sách chi tiết hóa đơn
            List<TChiTietHdb> chiTietHDBs = new List<TChiTietHdb>();

            THoaDonBan hoaDonBan = new THoaDonBan
            {
                NgayHoaDon = currentDay,
                TongTienHd = tongTienHoaDon,
                username = username,
                PhuongThucThanhToan = "vnpay"
            };

            db.THoaDonBans.Add(hoaDonBan);
            db.SaveChanges();

            var foundInvoice = db.THoaDonBans.FirstOrDefault(hd => hd.NgayHoaDon == currentDay && hd.username == username);

            if (foundInvoice != null)
            {
                foreach (var cartItem in userCartItems)
                {

                    TChiTietHdb chiTietHDB = new TChiTietHdb
                    {
                        MaHoaDon = foundInvoice.MaHoaDon,
                        MaSp = cartItem.MaSp,
                        SoLuongBan = cartItem.SoLuong,
                        DonGiaBan = cartItem.DonGia,
                    };

                    chiTietHDBs.Add(chiTietHDB);
                }
            }

            db.TChiTietHdbs.AddRange(chiTietHDBs);
            db.SaveChanges();

            db.TGioHangs.RemoveRange(userCartItems);
            db.SaveChanges();

            ViewBag.PaymentMethod = "Đã thanh toán bằng VNPay";

            return View("CheckoutSuccess");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}