using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliticalPortfolio.Controllers
{
    public class AlbumController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();
        public ActionResult Index()
        {
            var Albumitem = db.AlbumTbls.Where(v => v.IsActive == 1).ToList().OrderByDescending(v => v.Id);
            return View(Albumitem);
        }
        public ActionResult Gallery(long? id)
        {
            var Albumitem = db.GalleryTbls.Where(v => v.IsActive == 1 && v.AlbumID == id).ToList();
            return View(Albumitem);
        }
    }
}