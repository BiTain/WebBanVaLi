using Microsoft.AspNetCore.Mvc;

namespace WebBanVaLi.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
