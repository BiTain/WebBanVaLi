using WebBanVaLi.Models;
using Microsoft.AspNetCore.Mvc;
using WebBanVaLi.Repository;

namespace WebBanVaLi.ViewComponents
{
    public class LoaiSpMenuViewComponent : ViewComponent
    {
        private readonly ILoaiSpRepository _loaiSp;
        public LoaiSpMenuViewComponent(ILoaiSpRepository loaiSpRepository)
        {
            _loaiSp = loaiSpRepository;
        }
        public IViewComponentResult Invoke()
        {
            var loaisp=_loaiSp.GetAllLoaiSp().OrderBy(x=>x.Loai);
            return View(loaisp);
        }
    }
}
