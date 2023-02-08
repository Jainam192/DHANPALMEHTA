using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliticalPortfolio.Controllers
{
    public class NewsController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();
        public ActionResult Index()
        {
            var News = db.NewsTbls.Where(n => n.IsActive == 1).ToList().OrderByDescending(n => n.Id);
            return View(News);
        }
        public ActionResult NewsDetails(long? id)
        {
            dynamic mydata = new ExpandoObject();
            mydata.News = db.NewsTbls.Where(e => e.IsActive == 1 && e.Id == id).ToList().OrderByDescending(e => e.Id);
            mydata.RelatedNews = db.NewsTbls.Where(g => g.IsActive == 1 && g.Id != id).ToList().OrderByDescending(g => g.Id).Take(4);
            return View(mydata);
        }
    }
}