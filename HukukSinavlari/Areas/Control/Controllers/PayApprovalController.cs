using Business.Concrete;
using HukukSinavlari.Areas.Control.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HukukSinavlari.Areas.Control.Controllers
{
	[Area("Control")]
	[Authorize]
	public class PayApprovalController : Controller
	{
		private readonly PurchasedTrainingsManager _purchased;

		public PayApprovalController(PurchasedTrainingsManager purchased)
		{
			_purchased = purchased;
		}

		public IActionResult Index()
		{
			var values = _purchased.TGetList(x => x.PaymentType == "Havale").Include(x => x.Education)
				.Select(x => new RemittanceIncomingPaymentsModel
				{
					Id = x.Id,
					EducationName = x.Education.EducationName,
					PaymentType = x.PaymentType,
					PaymentStatus = x.PaymentStatus,
					Price = x.Education.Price,
					StartDate = x.Education.StartDate,
				}).ToList();
			return View(values);
		}

		public JsonResult Approved(int id)
		{
			var values = _purchased.TGetList(x => x.Id == id).ToList();
            foreach (var item in values)
            {
				item.PaymentStatus = true;
				_purchased.TUpdate(item);
            }
			return Json(values);
        }
	}
}
