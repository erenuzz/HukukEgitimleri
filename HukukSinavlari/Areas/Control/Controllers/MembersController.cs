using Business.Concrete;
using Entity.Entities;
using HukukSinavlari.Areas.Control.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace HukukSinavlari.Areas.Control.Controllers
{
    [Area("Control")]
    [Authorize]
    public class MembersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly PurchasedTrainingsManager _purchasedTrainingsManager;

        public MembersController(UserManager<AppUser> userManager, PurchasedTrainingsManager purchasedTrainingsManager)
        {
            _userManager = userManager;
            _purchasedTrainingsManager = purchasedTrainingsManager;
        }


        #region Üyeleri Listeleme
        public async Task<IActionResult> Index(int page=1)
        {
            var users = await _userManager.Users.ToListAsync();
            var values = new List<MembersListModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Any(r => r == "Ogrenci"))
                {
                    values.Add(new MembersListModel
                    {
                        Email = user.Email,
                        Id = user.Id,
                        Name = user.Name,
                        Phone = user.PhoneNumber,
                        Surname = user.Surname,
                        Address = user.Address,
                        Role = roles.ToArray(),
                        State = user.State
                        
                    });
                }
            }

            var members = values.ToPagedList(page, 10);
            return View(members);
        }
        #endregion


        #region Üye detayları
        [HttpGet]
        public IActionResult MembersCourses(int id)
        {
            var values = _purchasedTrainingsManager.TGetList(x => x.AppUserId == id && x.CanceledTrainings == false).Include(x => x.Education)
               .Select(x => new PurchasedEducationModel
               {
                   EducationName = x.Education.EducationName,
                   PaymentStatus = x.PaymentStatus,
                   EducationStatus = x.EducationStatus,
                   Id = x.Id,
                   Price = x.Education.Price
               }).ToList();
            return Json(values);
        }
        #endregion


        #region Üye durumu güncelleme
        public async Task<IActionResult> Edit(int id, bool durum)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                user.State = durum;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { message = "Öğrenci durumu değiştirildi" });
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
        #endregion

        public async Task<IActionResult> DeleteMembers(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if(user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { message = "Kayıt silindi" });
                }
                else
                {
                    return Json(new {message = "Kayıt silinirken bir hata oluştu"});
                }
            }
            else
            {
                return Json(new {message = "Kullanıcı bulunamadı."});
            }
        }

    }
}
