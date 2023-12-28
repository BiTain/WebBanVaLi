using Microsoft.AspNetCore.Mvc;

namespace WebBanVaLi.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
