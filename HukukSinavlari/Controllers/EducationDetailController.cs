using Business.Concrete;
using Entity.Entities;
using HukukSinavlari.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HukukSinavlari.Controllers
{
    public class EducationDetailController : Controller
    {
        private readonly EducationManager _educationManager;
       
        public EducationDetailController(EducationManager educationManager)
        {
            _educationManager = educationManager;
           
        }

        public IActionResult Index(int id)
        {
            var educationDetail = _educationManager.TGetList(x=>x.Id == id)
                .Select(x=>new EducationDetailModel
                {
                    Description = x.Description,
                    EducationName = x.EducationName,
                    Id = x.Id,                   
                    StartDate = x.StartDate,
                    Price = x.Price,
                    Image = x.Image,
                  
                }).FirstOrDefault();
           
            return View(educationDetail);
        }

        public IActionResult AddSessionEducation(int id)
        {
            HttpContext.Session.SetInt32("educationId", id);
            return Json(new { id = id });
        }
        
    }
}
