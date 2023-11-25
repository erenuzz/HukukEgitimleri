using Business.Concrete;
using Entity.Entities;
using HukukSinavlari.Areas.Control.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using static System.Net.WebRequestMethods;

namespace HukukSinavlari.Areas.Control.Controllers
{
    [Area("Control")]
    [Authorize]
    public class TrainerController : Controller
    {
       
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly TrainerLessonsManager _trainerLessonsManager;

        public TrainerController(UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, TrainerLessonsManager trainerlessManager)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _trainerLessonsManager = trainerlessManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {

            var users = await _userManager.Users.ToListAsync();
            var values = new List<TrainerListModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Any(r => r == "Egitmen"))
                {
                    values.Add(new TrainerListModel
                    {
                        Email = user.Email,
                        Id = user.Id,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        Surname = user.Surname, 
                        State = user.State,
                        Role = roles.ToArray()
                    });
                }
            }

            var trainer = values.ToPagedList(page, 10);

            return View(trainer);

        }


        [HttpPost]
        public async Task<IActionResult> AddTrainer(AddTrainerModel model)
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

                };
                var emailControl = _userManager.Users.FirstOrDefault(x => x.Mail == model.Mail);
                if (emailControl == null)
                {
                    var result = await _userManager.CreateAsync(appUser, model.Password);
                    if (result.Succeeded)
                    {
                        var role = await _userManager.AddToRoleAsync(appUser, "Egitmen");

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
            return Ok();
        }

        [HttpGet]
        public IActionResult TrainerEducation(int trainerId)
        {
          //  int userId = Convert.ToInt32(_contextAccessor.HttpContext.Session.GetInt32("userId"));

            var values = _trainerLessonsManager.TGetList(x => x.TrainerId == trainerId).Include(x => x.Education)
               .Select(x => new TrainerEducationModel
               {
                  // Id = x.Education.Id,
                   EducationName = x.Education.EducationName,
                   StartDate =x.Education.StartDate,
                   Image = x.Education.Image,
                 
               }).ToList();
            return Json(values);
        }

        public async Task<IActionResult> TrainerPassive(int id)
        {
            var user =await _userManager.FindByIdAsync(id.ToString());
            if (user !=null)
            {
                user.State = false;
                var result = await _userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    TempData["success"] = "Eğitmen durumu pasifleştirildi.";
                    return Redirect("/Control/Trainer/Index/");
                    
                }
                else 
                {
                    return View();
                }
            }
            else
            {
                TempData["message"] = "İşlem sırasında bir hata oluştu";
            }

            return View();
            
             
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TrainerEditModel model , int id)
        {
            var user =await _userManager.FindByIdAsync(id.ToString());
            if(user !=null)
            {
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.PhoneNumber = model.Phone;
                user.UserName = model.Mail;
                user.Mail = model.Mail;
                user.Email = model.Mail;
                user.Address = model.Address;
                user.State = model.State;

                var result = await _userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    return Json(user);
                }
                else
                {
                    return Json(new { state = true });
                }

            }
            else
            {
                return Json(new { state = false });
            }
        }

    }
}
