using Entity.Entities;
using HukukSinavlari.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace HukukSinavlari.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _http;

        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IHttpContextAccessor http)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _http = http;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserLoginModel loginModel)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);

                if (user != null)
                {
                    if (user.State == true)
                    {
                        var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, true);
                        if (result.Succeeded)
                        {

                            var roles = await _userManager.GetRolesAsync(user);
                            HttpContext.Session.SetString("userRole", roles.FirstOrDefault());

                            HttpContext.Session.SetInt32("userId", user.Id);
                            HttpContext.Session.SetString("userName", user.UserName);

                            int? educationId = _http.HttpContext.Session.GetInt32("educationId");
                            if (educationId > 0)
                            {
                                return Redirect("/Payment/Index/" + educationId);
                            }
                            else
                            {
                                return Redirect("/Home/Index");
                            }

                        }
                        else
                        {
                            ViewBag.hata = "Hatalı giriş lütfen tekrar deneyin";
                        }
                    }

                    else
                    {
                        ViewBag.error = "Hesabınız pasif hale getirilmiş. Lütfen yönetici ile iletişime geçin";
                        return View();
                    }
                }
                else
                {
                    ViewBag.kullanici = "Böyle bir hesap bulunamadı";
                }

            }


            return View();
        }


        public async Task<IActionResult> GetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                var password = user.UnencryptedPass;
                MailMessage msg = new MailMessage(); //Mesaj gövdesini tanımlıyoruz...
                msg.Subject = "Şifreniz";
                msg.From = new MailAddress("beoerp00@gmail.com", "Piksel Bilişim");
                msg.To.Add(new MailAddress(email, user.Name));
                msg.IsBodyHtml = true;
                msg.Body = "Şifreniz:" + " " + password;
                msg.Priority = MailPriority.High;

                //SMTP/Gönderici bilgilerinin yer aldığı erişim/doğrulama bilgileri
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587); //Bu alanda gönderim yapacak hizmetin smtp adresini ve size verilen portu girmelisiniz.
                NetworkCredential AccountInfo = new NetworkCredential("beoerp00@gmail.com", "zicihvcwnbiiugbw");
                smtp.UseDefaultCredentials = false; //Standart doğrulama kullanılsın mı? -> Yalnızca gönderici özellikle istiyor ise TRUE işaretlenir.
                smtp.Credentials = AccountInfo;
                smtp.EnableSsl = true; //SSL kullanılarak mı gönderilsin...
                smtp.Send(msg);
            }
            else
            {


                return Json(new { message = true });
            }
            return Json(email);          
        }




        //[HttpPost]
        //public async Task<IActionResult> Index(UserLoginModel loginModel, string ReturnUrl = "/")
        //{              

        //    if (ModelState.IsValid)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, true);
        //        if (result.Succeeded)
        //        {
        //            var user = await _userManager.FindByNameAsync(loginModel.UserName);
        //            var roles = await _userManager.GetRolesAsync(user);
        //            HttpContext.Session.SetString("userRole", roles.FirstOrDefault());

        //            HttpContext.Session.SetInt32("userId", user.Id);
        //            HttpContext.Session.SetString("userName", user.UserName);

        //            int? educationId = _http.HttpContext.Session.GetInt32("educationId");
        //            if (educationId > 0)
        //            {
        //                return Redirect("/Payment/Index/" + educationId);
        //            }
        //            else
        //            {
        //                return Redirect("/Home/Index");
        //            }

        //        }
        //        else
        //        {
        //            return View();
        //        }
        //    }


        //    return View();
        //}





        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _http.HttpContext.Session.Clear();
            return Redirect("/Home/Index/");
        }
    }
}
