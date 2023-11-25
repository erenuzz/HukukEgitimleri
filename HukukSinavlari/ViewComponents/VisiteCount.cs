using DataAccess.Context;
using HukukSinavlari.Models;
using Microsoft.AspNetCore.Mvc;

namespace HukukSinavlari.ViewComponents
{
    public class VisiteCount : ViewComponent
    {
        private readonly DbHukukEgitim _c;

        public VisiteCount(DbHukukEgitim c)
        {
            _c = c;
        }

        public IViewComponentResult Invoke()
        {
            var cityCounts = _c.onlineUsers
         .GroupBy(x => x.Location)
         .Select(group => new VisitCountModel
         {
             City = group.Key,
             Count = group.Count()
         })
         .ToList();

            return View(cityCounts);
        }


    }
}
