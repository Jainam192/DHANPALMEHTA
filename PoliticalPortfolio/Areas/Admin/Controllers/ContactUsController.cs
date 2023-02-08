using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PoliticalPortfolio;
using PoliticalPortfolio.Areas.Admin.Data;

namespace PoliticalPortfolio.Areas.Admin.Controllers
{
    [AdminSessionCheck]
    public class ContactUsController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();

        public ActionResult Index()
        {
            return View(db.ContactUsTbls.Where(c=> c.IsActive == 1).ToList().OrderByDescending(c=> c.Id));
        }

    
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
