using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliticalPortfolio.Controllers
{
    public class ContactUsController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ContactUsTbl contactUsTbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contactUsTbl.Id = (db.ContactUsTbls.Select(x => (long?)x.Id).Max() ?? 0) + 1;
                    contactUsTbl.IsActive = 1;
                    contactUsTbl.CreateDate = DateTime.Now;
                    db.ContactUsTbls.Add(contactUsTbl);
                    TempData["SuccessMessage"] = "Your Enquiry Saved Successfully";
                    db.SaveChanges();
                }
                else
                {
                    TempData["FailMessage"] = "Record Field";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["FailMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}