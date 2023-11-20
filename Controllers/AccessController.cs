using Microsoft.AspNetCore.Mvc;
using WebBanVaLi.Models;
using WebBanVaLi.Models.RegisterModel;

namespace WebBanVaLi.Controllers
{
    public class AccessController : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(TUser user)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var u = db.TUsers.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password)).FirstOrDefault();
                if (u != null && u.LoaiUser == 0)
                {
                    HttpContext.Session.SetString("UserName", u.Username.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else if (u != null && u.LoaiUser == 1)
                {
                    HttpContext.Session.SetString("UserName", u.Username.ToString());
                    return RedirectToAction("Index", "Admin");
                }
            }
            ViewBag.ErrorLogin = "Username or password is incorrect";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Access");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Register register)
        {

            var user = db.TUsers.Where(u => u.Username.Equals(register.username)).FirstOrDefault();
            if (user != null || register.username == null)
                ViewBag.ErrorName = "Tên người dùng đã tồn tại";

            if (!register.password.Equals(register.rePassword))
                ViewBag.ErrorPass = "Mật khẩu không khớp";
            if (ViewBag.ErrorName != null || ViewBag.ErrorPass != null)
                return View();
            db.TUsers.Add(new TUser
            {
                Username = register.username,
                Password = register.password,
                LoaiUser = 0
            });
            db.SaveChanges();
            return RedirectToAction("Login", "Access");
        }

    }
}
