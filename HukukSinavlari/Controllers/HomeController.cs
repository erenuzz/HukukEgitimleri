using HukukSinavlari.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using DataAccess.Context;
using System.Net.Sockets;
using HukukSinavlari.Helper;
using Entity.Entities;
using Business.Concrete;
using HukukSinavlari.Areas.Control.Models;
using Microsoft.EntityFrameworkCore;

namespace HukukSinavlari.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly db _c;
        private readonly IpStackLocationService _locationService;
        private readonly EducationManager _educationManager;
       
        public HomeController(ILogger<HomeController> logger , db c , IpStackLocationService locationService , EducationManager educationManager)
        {
            _logger = logger;
            _c = c;
            _locationService = locationService;
            _educationManager = educationManager;
        }

        public IActionResult Index()
        {
        
            var values = _educationManager.TGetList()
                .Select(x => new EducationListModel
                {
                    Id = x.Id,
                    EducationName = x.EducationName,
                    StartDate = x.StartDate,                    
                    Price = x.Price,
                    Description = x.Description,  
                    Image = x.Image,
                    
                }).ToList();

            return View(values);
        }


        public string GetClientIpAddress(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress;
            var forwardedHeader = context.Request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                var forwardedHeaderValue = forwardedHeader.ToString();
                ipAddress = IPAddress.Parse(forwardedHeaderValue.Split(',')[0]);
            }

            return ipAddress.ToString();
        }

        public IpStackLocation GetLocationFromIp(string ipAddress)
        {
            var location = _locationService.GetLocationFromIpAsync(ipAddress).Result;
            return location;
        }

        [HttpPost]
        public IActionResult SendMail(string ad, string soyad, string gonderenMail, string telNo , string mesaj , string egitim)
        {
            MailMessage msg = new MailMessage(); //Mesaj gövdesini tanımlıyoruz...
            msg.Subject = "Eğitim bilgi talebi";
            msg.From = new MailAddress(gonderenMail, ad + " " + soyad);
            msg.To.Add(new MailAddress("", " "));
            //contact@mysardis.com
            msg.IsBodyHtml = true;
            msg.Body = $"<p><strong>Ad:</strong> {ad}</p>" +
            $"<p><strong>Soyad:</strong> {soyad}</p>" +
            $"<p><strong>Telefon Numarası:</strong> {telNo}</p>" +
            $"<p><strong>Seçilen Eğitim:</strong> {egitim}</p>" +            
            $"<p><strong>Mesaj:</strong> {mesaj}</p>";
            msg.Priority = MailPriority.High;
            //SMTP/Gönderici bilgilerinin yer aldığı erişim/doğrulama bilgileri
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587); //Bu alanda gönderim yapacak hizmetin smtp adresini ve size verilen portu girmelisiniz.
            NetworkCredential AccountInfo = new NetworkCredential("", "");
            smtp.UseDefaultCredentials = false; //Standart doğrulama kullanılsın mı? -> Yalnızca gönderici özellikle istiyor ise TRUE işaretlenir.
            smtp.Credentials = AccountInfo;
            smtp.EnableSsl = true; //SSL kullanılarak mı gönderilsin...
            smtp.Send(msg);

            return Ok();
        }



    }
}
