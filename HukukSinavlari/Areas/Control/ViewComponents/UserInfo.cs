using Microsoft.AspNetCore.Mvc;

namespace HukukSinavlari.Areas.Control.ViewComponents
{
    public class UserInfo:ViewComponent
    {
        private readonly IHttpContextAccessor _http;

        public UserInfo(IHttpContextAccessor http)
        {
            _http = http;
        }

        public IViewComponentResult Invoke()
        {
            var userName = _http.HttpContext.Session.GetString("userName");
            var role = _http.HttpContext.Session.GetString("userRole");
            ViewBag.userName = userName;
            ViewBag.role = role;
            return View();
        }
    }
}
