using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PoliticalPortfolio;
using PoliticalPortfolio.Areas.Admin.Data;

namespace PoliticalPortfolio.Areas.Admin.Controllers
{
    [AdminSessionCheck]
    public class EventController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();

        public ActionResult Index()
        {
            return View(db.EventTbls.Where(e=> e.IsActive == 1).ToList().OrderByDescending(e=> e.Id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,ImagePath,Time,Venue,EventDate,CreateDate,ModifiedDate,IsActive")] EventTbl eventTbl , HttpPostedFileBase ImagePath)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ImagePath != null)
                    {
                        string nowdatetime = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute;
                        eventTbl.ImagePath = "/Files/EventImages/" + nowdatetime + Path.GetFileName(ImagePath.FileName);
                        ImagePath.SaveAs(Server.MapPath("~" + eventTbl.ImagePath));
                        eventTbl.Id = (db.EventTbls.Select(x => (long?)x.Id).Max() ?? 0) + 1;
                        eventTbl.CreateDate = DateTime.Now;
                        eventTbl.IsActive = 1;
                        db.EventTbls.Add(eventTbl);
                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Record Saved Successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["FailMessage"] = "Please Select Image";
                        return RedirectToAction("Index");
                    }

                }
                return View(eventTbl);
            }
            catch (Exception ex)
            {
                TempData["FailMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventTbl eventTbl = db.EventTbls.Find(id);
            if (eventTbl == null)
            {
                return HttpNotFound();
            }
            return View(eventTbl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,ImagePath,Time,Venue,EventDate,CreateDate,ModifiedDate,IsActive")] EventTbl eventTbl , HttpPostedFileBase ImagePath )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ImagePath != null)
                    {
                        string nowdatetime = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute;
                        eventTbl.ImagePath = "/Files/EventImages/" + nowdatetime + Path.GetFileName(ImagePath.FileName);
                        ImagePath.SaveAs(Server.MapPath("~" + eventTbl.ImagePath));
                    }
                    eventTbl.ModifiedDate = DateTime.Now;
                    db.Entry(eventTbl).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Record Update Successfully";
                    return RedirectToAction("Index");
                }
                return View(eventTbl);
            }
            catch (Exception ex)
            {
                TempData["FailMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            try
            {
                string datenow = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                int noOfRowUpdated = db.Database.ExecuteSqlCommand("Update EventTbl set IsActive='0',ModifiedDate='" + datenow + "' where Id = " + id + "");
                if (noOfRowUpdated == 0)
                {
                    TempData["FailMessage"] = "No Row Deleted";
                    return RedirectToAction("Index");
                }
                TempData["SuccessMessage"] = "Record Deleted Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["FailMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
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
