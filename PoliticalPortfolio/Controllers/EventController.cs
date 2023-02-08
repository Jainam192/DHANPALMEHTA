using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliticalPortfolio.Controllers
{
    public class EventController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();
        public ActionResult Index()
        {
            var Eventitem = db.EventTbls.Where(e => e.IsActive == 1).ToList().OrderByDescending(e => e.Id);
            return View(Eventitem);
        }
        public ActionResult EventDetails(long? id)
        {
            dynamic mydata = new ExpandoObject();
            mydata.Event = db.EventTbls.Where(e => e.IsActive == 1 && e.Id == id).ToList().OrderByDescending(e => e.Id);
            mydata.RelatedEvent = db.EventTbls.Where(g => g.IsActive == 1 && g.Id != id).ToList().OrderByDescending(g => g.Id).Take(3);
            return View(mydata);
        }
    }
}