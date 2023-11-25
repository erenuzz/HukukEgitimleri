using Business.Concrete;
using Entity.Entities;
using HukukSinavlari.Areas.Control.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace HukukSinavlari.Areas.Control.Controllers
{
    [Area("Control")]
    [Authorize]
    public class PanelController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EducationManager _educationManager;
        private readonly IHttpContextAccessor _http;
        private readonly PurchasedTrainingsManager _purchasedTrainingsManager;
        private readonly TrainerLessonsManager _trainerLessons;

        public PanelController(UserManager<AppUser> userManager,EducationManager educationManager, IHttpContextAccessor http, PurchasedTrainingsManager purchasedTrainingsManager,TrainerLessonsManager trainerLessonsManager)
        {
            _userManager = userManager;
            _educationManager = educationManager;
            _http = http;
            _purchasedTrainingsManager = purchasedTrainingsManager;
            _trainerLessons = trainerLessonsManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region Rolü öğrenci olan üyelerin sayısı
        public async Task<IActionResult> UserCount()
        {
            var users = await _userManager.Users.ToListAsync();
            var values = new List<TrainerListModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any(r => r == "Ogrenci"))
                {
                    values.Add(new TrainerListModel
                    {
                        Role = roles.ToArray()
                    });
                }
            }
            return Json(values);
        }
        #endregion

        #region Eğitim Sayısı
        public IActionResult EducationCount()
        {
            var values = _educationManager.TGetList().Count();
            return Json(values);
        }
        #endregion

        #region Öğrencinin Aldıgı Eğitim Sayısı
        public IActionResult MyTrainings()
        {
            int userId = Convert.ToInt32(_http.HttpContext.Session.GetInt32("userId"));
            var values = _purchasedTrainingsManager.TGetList(x => x.AppUserId == userId).Count();
            return Json(values);
        }
        #endregion

        #region Eğitmenin Dersleri
        public IActionResult TrainingLessons()
        {
            var userId = _http.HttpContext.Session.GetInt32("userId");
            var values = _trainerLessons.TGetList(x => x.TrainerId == userId).Count();
            return Json(values);

        } 
        #endregion

    }
}
