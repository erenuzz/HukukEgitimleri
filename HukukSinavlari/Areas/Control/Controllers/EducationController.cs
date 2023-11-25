using Business.Concrete;
using Entity.Entities;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using HukukSinavlari.Areas.Control.Models;
using HukukSinavlari.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using X.PagedList;
using static System.Formats.Asn1.AsnWriter;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using System.Text;


namespace HukukSinavlari.Areas.Control.Controllers
{
    [Area("Control")]
    [Authorize]
    public class EducationController : Controller
    {
        #region Dependency
        private readonly EducationManager _educationManager;
        private readonly PurchasedTrainingsManager _purchasedTrainingsManager;
        private readonly IHttpContextAccessor _http;
        private readonly TrainerLessonsManager _trainerLessonsManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly TrainingHoursManager _trainingHoursManager;
      
        #endregion

        #region Ctor
        public EducationController(EducationManager educationManager, PurchasedTrainingsManager purchasedTrainingsManager, IHttpContextAccessor http, TrainerLessonsManager trainerLessonsManager, UserManager<AppUser> userManager, TrainingHoursManager trainingHoursManager)
        {
            _educationManager = educationManager;
            _purchasedTrainingsManager = purchasedTrainingsManager;
            _http = http;
            _trainerLessonsManager = trainerLessonsManager;
            _userManager = userManager;
            _trainingHoursManager = trainingHoursManager;

        } 
        #endregion

        #region Index
        public async Task<IActionResult> Index(int page = 1)
        {
            var values = _educationManager.TGetList(x=>x.Status == true).ToPagedList(page, 10)
                .Select(x => new EducationListModel
                {
                    Id = x.Id,
                    EducationName = x.EducationName,
                    StartDate = x.StartDate,
                    Price = x.Price,
                    Image = x.Image,
                    Description = x.Description,
                    // Link = x.Link
                    Status = x.Status,

                }).ToPagedList(page, 10);

            var finished = _educationManager.TGetList(x => x.Status == false).ToPagedList(page, 10)
                .Select(x => new EducationListModelFinished
                {
                    Id = x.Id,
                    EducationName = x.EducationName,
                    StartDate = x.StartDate,
                    Price = x.Price,
                    Image = x.Image,
                    Description = x.Description,
                    // Link = x.Link
                    Status = x.Status

                }).ToPagedList(page, 10);

            var model = (values, finished);

            return View(model);
        }
        #endregion

        #region Eğitimlerin katılımcıları
        //Eğitimlerin katılımcıları
        [HttpGet]
        public IActionResult Participants(int educationId)
        {
            var values = _purchasedTrainingsManager.TGetList(x => x.EducationId == educationId && x.CanceledTrainings == false && x.PaymentStatus == true).Include(x => x.AppUser)
               .Select(x => new EducationParticipantsModel
               {
                   Name = x.AppUser.Name,
                   Surname = x.AppUser.Surname,
                   Phone = x.AppUser.PhoneNumber,
                   Mail = x.AppUser.Mail
               }).ToList();
            return Json(values);
        }
        #endregion

        #region Eğitimlerin Hocaları
        //Eğitimlerin hocaları
        public IActionResult EducationTrainers(int educationId)
        {
            var values = _trainerLessonsManager.TGetList(x => x.EducationId == educationId && x.Status == true).Include(x => x.Trainer)
               .Select(x => new EducationTrainerModel
               {
                   Name = x.Trainer.Name,
                   Surname = x.Trainer.Surname,
                   Phone = x.Trainer.PhoneNumber,
                   Mail = x.Trainer.Mail
               }).ToList();
            return Json(values);
        }
        #endregion

        #region Öğrencilerin eğitimleri
        //UserId ye göre öğrenci kendi panelinde eğitimlerini görür
        public IActionResult Mytrainings()
        {
            int userId = Convert.ToInt32(_http.HttpContext.Session.GetInt32("userId"));
            var values = _purchasedTrainingsManager.TGetList(x => x.AppUserId == userId && x.CanceledTrainings == false && x.PaymentStatus == true).Include(x => x.Education)
                .Select(x => new PurchasedEducationModel
                {
                    Id = x.Education.Id,
                    EducationName = x.Education.EducationName,
                    StartDate = x.Education.StartDate,
                    EducationStatus = x.EducationStatus,
                    PaymentStatus = x.EducationStatus,
                    CanceledTrainings = x.CanceledTrainings,
                }).ToList();

            var canceledEducation = _purchasedTrainingsManager.TGetList(x => x.AppUserId == userId && x.CanceledTrainings == true).Include(x => x.Education)
               .Select(x => new CanceledEducationModel
               {
                   Id = x.Education.Id,
                   EducationName = x.Education.EducationName,
                   StartDate = x.Education.StartDate,
                   EducationStatus = x.EducationStatus,
                   PaymentStatus = x.EducationStatus,
                   CanceledTrainings = x.CanceledTrainings,

               }).ToList();

           

            var model = (values, canceledEducation);

            return View(model);
        }
        #endregion

        #region Seçili Eğitmen Listesi
        //Eğitmen Listesi
        public async Task<string> SelectedTrainerList(int Id)
        {
            var educationTrainers = _trainerLessonsManager.TGetList(x => x.EducationId == Id && x.Status == true);

            string selectOption = "";
            var users = await _userManager.Users.ToListAsync();
            var values = new List<TrainerListModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Any(r => r == "Egitmen"))
                {

                    string selectedItem = educationTrainers.Any(x => x.TrainerId == user.Id) ? "selected" : "";
                    selectOption += "<option value='" + user.Id + "' " + selectedItem + ">" + user.Name + "</option>";
                }
            }

            return selectOption;
        }
        #endregion

        #region Eğitmen Listesi
        public async Task<IActionResult> TrainerList()
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
                        Surname = user.Surname,
                        Role = roles.ToArray()
                    });

                }
            }

            return Json(values);
        }
        #endregion

        #region Eğitim Ekleme
        //Eğitim ekleme
        [HttpPost]
        public IActionResult Add(Education t, IFormFile Image, int[] trainerId)
        {
           
            Education education = new Education();
            if (Image != null)
            {
                var extension = Path.GetExtension(Image.FileName);
                var newimagename = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EducationImage/", newimagename);
                var stream = new FileStream(location, FileMode.Create);
                Image.CopyTo(stream);
                education.Image = "/EducationImage/" + newimagename;
            }
            education.CreateDate = DateTime.Now;
            education.EducationName = t.EducationName;
            education.StartDate = t.StartDate;
            education.Price = t.Price;
            education.Description = t.Description;
            education.Status = true;
            _educationManager.TAdd(education);

            int educationid = education.Id;

            for (int i = 0; i < trainerId.Length; i++)
            {
                TrainerLessons lessons = new TrainerLessons();
                lessons.EducationId = educationid;
                lessons.TrainerId = trainerId[i];
                lessons.Status = true;               
                _trainerLessonsManager.TAdd(lessons);
            }

            return Json(new { message = "Kayıt başarılı bir şekilde eklendi" });
        }
        #endregion

        #region Eğitmenin Dersleri
        [HttpGet]
        public IActionResult TrainerEducation(int trainerId)
        {
            int userId = Convert.ToInt32(_http.HttpContext.Session.GetInt32("userId"));

            var values = _trainerLessonsManager.TGetList(x => x.TrainerId == userId && x.Status == true).Include(x => x.Education)
               .Select(x => new TrainerEducationModel
               {
                   Id = x.Education.Id,
                   EducationName = x.Education.EducationName,
                   StartDate = x.Education.StartDate,
                   Image = x.Education.Image,

               }).ToList();


            var finishedEducation = _trainerLessonsManager.TGetList(x => x.TrainerId == userId && x.Status == false).Include(x => x.Education)
               .Select(x => new FinishedEducationModel
               {
                   Id = x.Education.Id,
                   EducationName = x.Education.EducationName,
                   StartDate = x.Education.StartDate,
                   Image = x.Education.Image,

               }).ToList();

            var model = (values, finishedEducation);

            return View(model);
        }
        #endregion

        #region Ders saati ayarlama
        [HttpGet]
        public IActionResult TrainingHours(int Id)
        {
            var values = _educationManager.TGetList(x => x.Id == Id)
                .Select(x => new EducationListModel
                {
                    Id = x.Id,
                    EducationName = x.EducationName,
                    StartDate = x.StartDate,
                    Price = x.Price,
                    Image = x.Image
                }).FirstOrDefault();

            return View(values);
        }
        //[HttpPost]
        //public IActionResult TrainingHours(int Id, TrainingHours trainingHours)
        //{
        //    TrainingHours training = new TrainingHours();
        //    training.Description = trainingHours.Description;
        //    training.StartDate = trainingHours.StartDate;
        //    training.EndDate = trainingHours.EndDate;            
        //    training.Link = trainingHours.Link;           
        //    training.EducationId = Id;
        //    _trainingHoursManager.TAdd(training);

        //    return Json(trainingHours);
        //}
        #endregion

        [HttpPost]
        public IActionResult TrainingHours(int Id, TrainingHours trainingHours)
        {
            try
            {
                // Google Calendar API'ı için gerekli konfigürasyon
                string clientId = "364557991426-fnv6l1t8lgg91i3ck4ciu67v3v0c8pkb.apps.googleusercontent.com";
                string clientSecret = "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";
                string redirectUri = "https://localhost:7175/";

                // Google Calendar Manager sınıfını oluştur
                GoogleCalendarManager calendarManager = new GoogleCalendarManager();
               

                // Google Calendar'a etkinlik eklemek için gerekli bilgileri oluştur
                Event newEvent = new Event
                {
                    Summary = trainingHours.Description,
                    Start = new EventDateTime { DateTime = trainingHours.StartDate, TimeZone = "Turkey Standard Time" },
                    End = new EventDateTime { DateTime = trainingHours.EndDate, TimeZone = "Turkey Standard Time" },
                    ConferenceData = new ConferenceData
                    {
                        CreateRequest = new CreateConferenceRequest
                        {
                            RequestId = Guid.NewGuid().ToString()
                        }
                    }
                };

                // Google Calendar'a etkinliği ekle
                string eventLink = calendarManager.AddEvent(newEvent);

                // Oluşturulan Google Meet linkini TrainingHours nesnesine ata
                trainingHours.Link = eventLink;

                // TrainingHours nesnesini veritabanına ekle
                TrainingHours training = new TrainingHours
                {
                    Description = trainingHours.Description,
                    StartDate = trainingHours.StartDate,
                    EndDate = trainingHours.EndDate,
                    Link = trainingHours.Link,
                    EducationId = Id
                };

                _trainingHoursManager.TAdd(training);

                return Json(trainingHours);
            }
            catch (Exception ex)
            {
                // Hata durumunda uygun bir işlem yapabilirsiniz
                return Json(new { error = ex.Message });
            }
        }


        #region Ders Saati Kontrolü
        public IActionResult TrainingHoursControl(int educationId)
        {
            var trainingHoursControl = _trainingHoursManager.TGetList(x => x.EducationId == educationId);
            return Json(trainingHoursControl);
        }
        #endregion

        #region Silme

        public IActionResult Delete(int id)
        {
            var values = _educationManager.TGetById(id);
            _educationManager.TDelete(values);
            return Redirect("/Control/Education/Index");
        }
        #endregion

        #region Edit

        [HttpPost]
        public IActionResult Edit(int Id, Education education, IFormFile Image, int[] trainerIdd)
        {
            var values = _educationManager.TGetById(Id);
            if (Image != null)
            {
                var extension = Path.GetExtension(Image.FileName);
                var newimagename = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EducationImage/", newimagename);
                var stream = new FileStream(location, FileMode.Create);
                Image.CopyTo(stream);
                values.Image = "/EducationImage/" + newimagename;
            }
            else
            {
                values.Image = values.Image;
            }
            values.EducationName = education.EducationName;
            if (education.StartDate != Convert.ToDateTime("1.01.0001 00:00:00"))
            {
                values.StartDate = education.StartDate;
            }
            else
            {
                values.StartDate = values.StartDate;
            }
            values.Status = values.Status;
            values.Price = education.Price;
            values.Description = education.Description;

            values.CreateDate = values.CreateDate;
            _educationManager.TUpdate(values);

            if (trainerIdd.Count() != 0)
            {

                var lessons = _trainerLessonsManager.TGetList(x => x.EducationId == Id).ToList();

                for (int i = 0; i < trainerIdd.Length; i++)
                {                    
                    var trainer = _trainerLessonsManager.TGetList(x => x.TrainerId == trainerIdd[i]).ToList();

                    if (trainer.Count == 0)
                    {
                        TrainerLessons lessons2 = new TrainerLessons();
                        lessons2.EducationId = Id;
                        lessons2.TrainerId = trainerIdd[i];
                        lessons2.Status = true;
                      
                        _trainerLessonsManager.TAdd(lessons2);
                    }

                }
                foreach (var item in lessons)
                {
                    item.Status = false;
                    _trainerLessonsManager.TUpdate(item);
                    
                }

                foreach (var item in lessons)
                {
                    for (int i = 0; i < trainerIdd.Count(); i++)
                    {

                        if (item.TrainerId == trainerIdd[i])
                        {

                            item.Status = true;
                            _trainerLessonsManager.TUpdate(item);

                        }


                    }
                }


                //for (var i = 0; i < trainerIdd.Length; i++)
                //{

                //    TrainerLessons item = new TrainerLessons();

                //    item.EducationId = Id;
                //    item.TrainerId = trainerIdd[i];
                //    item.Status = true;
                //    _trainerLessonsManager.TUpdate(item);

                //}


                return Json(new { message = "..." });
            }
            else
            {
                var lessons = _trainerLessonsManager.TGetList(x => x.EducationId == Id).ToList();
                for (int i = 0; i < lessons.Count; i++)
                {
                    lessons[i].EducationId = Id;
                    lessons[i].TrainerId = trainerIdd[i];
                    _trainerLessonsManager.TUpdate(lessons[i]);
                }


                return Json(new { message = "..." });
            }
        }

        #endregion

        #region EgitmenListele
        public IActionResult SelectedTrainer(int educationId)
        {
            var values = _trainerLessonsManager.TGetList(x => x.EducationId == educationId).ToList();
            return Json(values);
        }
        #endregion

        #region Ders Linki
        public IActionResult PlayVideo(int Id)
        {
            var values = _trainingHoursManager.TGetList(x => x.EducationId == Id).ToList();
            string linkUrl = "";
            foreach (var item in values)
            {
                linkUrl += item.Link;
            }
            return Redirect(linkUrl);

        }
        #endregion

        #region Eğitim Bitirme
        public JsonResult FinishTraining(int Id)
        {
            var values = _educationManager.TGetById(Id);
            values.Status = false;
            _educationManager.TUpdate(values);

            var education = _trainerLessonsManager.TGetList(x => x.EducationId == Id).ToList();
            foreach (var item in education)
            {
                item.Status = false;
                _trainerLessonsManager.TUpdate(item);
            }

            var purchasedEducation = _purchasedTrainingsManager.TGetList(x => x.EducationId == Id).ToList();
            foreach (var item in purchasedEducation)
            {
                item.EducationStatus = false;
                _purchasedTrainingsManager.TUpdate(item);
            }

            return Json(values);
        }
        #endregion

        #region Eğitimi İptal Etme
        public JsonResult CanceledEducation(int id)
        {
            //Eğitim iptal edildi ödeme geri geldi. ve eğitim iptal edilen eğitimler sayfasına düştü
            var values = _purchasedTrainingsManager.TGetList(x => x.EducationId == id).ToList();
            foreach (var item in values)
            {
                item.CanceledTrainings = true;
                _purchasedTrainingsManager.TUpdate(item);
            }
            return Json(values);
        }
        #endregion


     

    }
        
}
