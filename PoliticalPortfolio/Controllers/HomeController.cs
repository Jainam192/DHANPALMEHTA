using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PoliticalPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();
        public ActionResult Index()
        {
            dynamic mydata = new ExpandoObject();
            mydata.Event = db.EventTbls.Where(g => g.IsActive == 1).ToList().OrderByDescending(g => g.Id).Take(4);
            mydata.Video = db.VideoTbls.Where(g => g.IsActive == 1).ToList().OrderByDescending(g => g.Id).Take(4);
            mydata.Album = db.GalleryTbls.Where(g => g.IsActive == 1).ToList().OrderByDescending(g => g.Id).Take(6);
            mydata.News = db.NewsTbls.Where(g => g.IsActive == 1).ToList().OrderByDescending(g => g.Id).Take(4);
            mydata.Slider = db.SliderTbls.Where(g => g.IsActive == 1).ToList().OrderByDescending(g => g.Id).Take(3);
            return View(mydata);
        }
    }
}