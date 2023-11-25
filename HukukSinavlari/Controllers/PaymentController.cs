using Business.Concrete;
using Entity.Entities;
using HukukSinavlari.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HukukSinavlari.Controllers
{
	public class PaymentController : Controller
	{
		private readonly EducationManager _educationManager;
		private readonly PurchasedTrainingsManager _purchasedTrainingsManager;
		private readonly IHttpContextAccessor _http;

		public PaymentController(EducationManager educationManager, PurchasedTrainingsManager purchasedTrainingsManager, IHttpContextAccessor http)
		{
			_educationManager = educationManager;
			_purchasedTrainingsManager = purchasedTrainingsManager;
			_http = http;
		}

		public IActionResult Index(int id)
		{
			var educationDetail = _educationManager.TGetList(x => x.Id == id)
				 .Select(x => new EducationDetailModel
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

		[HttpPost]
		public IActionResult Pay(int educationId , string PaymentType)
		{
			PurchasedTrainings purchased = new PurchasedTrainings();
			int userId = Convert.ToInt32(_http.HttpContext.Session.GetInt32("userId"));
			purchased.AppUserId = userId;
			purchased.EducationId = educationId;
			purchased.CreateDate = DateTime.Now;

			if(PaymentType == "Havale")
			{
                purchased.PaymentStatus = false;
            }
			else
			{
                purchased.PaymentStatus = true;
            }			

			purchased.EducationStatus = true;
			purchased.CanceledTrainings = false;
			purchased.PaymentType = PaymentType;
            _purchasedTrainingsManager.TAdd(purchased);
			return Json(purchased);

		}
	}
}
