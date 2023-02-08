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
    public class SliderController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();

        public ActionResult Index()
        {
            return View(db.SliderTbls.Where(s=> s.IsActive == 1).ToList().OrderByDescending(s=> s.Id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ImagePath,CreateDate,IsActive")] SliderTbl sliderTbl , HttpPostedFileBase ImagePath)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ImagePath != null)
                    {
                        string nowdatetime = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute;
                        sliderTbl.ImagePath = "/Files/SliderImages/" + nowdatetime + Path.GetFileName(ImagePath.FileName);
                        ImagePath.SaveAs(Server.MapPath("~" + sliderTbl.ImagePath));
                        sliderTbl.Id = (db.SliderTbls.Select(x => (long?)x.Id).Max() ?? 0) + 1;
                        sliderTbl.CreateDate = DateTime.Now;
                        sliderTbl.IsActive = 1;
                        db.SliderTbls.Add(sliderTbl);
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
                return View(sliderTbl);
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
            SliderTbl sliderTbl = db.SliderTbls.Find(id);
            if (sliderTbl == null)
            {
                return HttpNotFound();
            }
            return View(sliderTbl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ImagePath,CreateDate,IsActive")] SliderTbl sliderTbl,HttpPostedFileBase ImagePath)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ImagePath != null)
                    {
                        string nowdatetime = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute;
                        sliderTbl.ImagePath = "/Files/SliderImages/" + nowdatetime + Path.GetFileName(ImagePath.FileName);
                        ImagePath.SaveAs(Server.MapPath("~" + sliderTbl.ImagePath));
                    }
                    db.Entry(sliderTbl).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Record Update Successfully";
                    return RedirectToAction("Index");
                }
                return View(sliderTbl);
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
                int noOfRowUpdated = db.Database.ExecuteSqlCommand("Update SliderTbl set IsActive='0' where Id = " + id + "");
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
