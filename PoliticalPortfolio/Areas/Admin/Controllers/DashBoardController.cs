using PoliticalPortfolio.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliticalPortfolio.Areas.Admin.Controllers
{
    [AdminSessionCheck]
    public class DashBoardController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();
        public ActionResult Index()
        {
            ViewBag.Album = db.AlbumTbls.Where(a => a.IsActive == 1).Count();
            ViewBag.Video = db.VideoTbls.Where(a => a.IsActive == 1).Count();
            ViewBag.News = db.NewsTbls.Where(a => a.IsActive == 1).Count();
            ViewBag.Event = db.EventTbls.Where(a => a.IsActive == 1).Count();
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}