using DataAccess.Context;
using Entity.Entities;
using HukukSinavlari.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HukukSinavlari.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _role;

        public RoleController(RoleManager<AppRole> role)
        {
            _role = role;
        }

        
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                AppRole role = new AppRole
                {
                    Name = model.Name,
                };
                var result = await _role.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Json(result);
                }
                else
                {
                    return Json(new { message = "Rol eklenemedi" });
                }
            }
            return View();
        }
    }
}
