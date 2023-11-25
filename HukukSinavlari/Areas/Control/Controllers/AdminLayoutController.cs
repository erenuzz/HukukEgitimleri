using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HukukSinavlari.Areas.Control.Controllers
{
    [Area("Control")]
    [Authorize]
    public class AdminLayoutController : Controller
    {
        

        public PartialViewResult Header()
        {
           
            return PartialView();
        }

        public PartialViewResult SideBar()
        {
            return PartialView();
        }

    }
}
