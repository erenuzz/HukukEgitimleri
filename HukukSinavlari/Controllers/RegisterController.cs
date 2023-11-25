using Entity.Entities;
using HukukSinavlari.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HukukSinavlari.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    UserName = model.Mail,
                    Mail = model.Mail,
                    Address = model.Address,
                    Email = model.Mail,
                    CreateDate = DateTime.Now,
                    PhoneNumber = model.Phone,
                    State = true,
                    UnencryptedPass = model.Password

                };
                var emailControl = _userManager.Users.FirstOrDefault(x => x.Mail == model.Mail);
                if (emailControl == null)
                {
                    var result = await _userManager.CreateAsync(appUser, model.Password);
                    if (result.Succeeded)
                    {
                        var role = await _userManager.AddToRoleAsync(appUser, "Ogrenci");

                    }
                    else
                    {
                        return Json(new { message = "Kullanıcı kaydedilirken hata oluştu." });
                    }
                }
                else
                {
                    return Json(new { message = "Bu email adresi ile daha önce kayıt olunmuş.Şifrenizi unuttuysanız eğer yeni şifre talep edin." });
                }
            }
            return View();
        }
    }
}
