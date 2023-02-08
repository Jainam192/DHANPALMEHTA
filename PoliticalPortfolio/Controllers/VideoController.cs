using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliticalPortfolio.Controllers
{
    public class VideoController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();
        public ActionResult Index()
        {
            var Video = db.VideoTbls.Where(v => v.IsActive == 1).ToList().OrderByDescending(v => v.Id);
            return View(Video);
        }
    }
}