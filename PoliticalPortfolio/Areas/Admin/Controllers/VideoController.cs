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
    public class VideoController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();

        public ActionResult Index()
        {
            return View(db.VideoTbls.Where(v=> v.IsActive == 1).ToList().OrderByDescending(v=> v.Id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Url,CoverImage,CreateDate,ModifiedDate,IsActive")] VideoTbl videoTbl , HttpPostedFileBase CoverImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (CoverImage != null)
                    {
                        string nowdatetime = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute;
                        videoTbl.CoverImage = "/Files/VideoCoverImage/" + nowdatetime + Path.GetFileName(CoverImage.FileName);
                        CoverImage.SaveAs(Server.MapPath("~" + videoTbl.CoverImage));
                        videoTbl.Id = (db.VideoTbls.Select(x => (long?)x.Id).Max() ?? 0) + 1;
                        videoTbl.CreateDate = DateTime.Now;
                        videoTbl.IsActive = 1;
                        db.VideoTbls.Add(videoTbl);
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
                return View(videoTbl);
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
            VideoTbl videoTbl = db.VideoTbls.Find(id);
            if (videoTbl == null)
            {
                return HttpNotFound();
            }
            return View(videoTbl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Url,CoverImage,CreateDate,ModifiedDate,IsActive")] VideoTbl videoTbl , HttpPostedFileBase CoverImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (CoverImage != null)
                    {
                        string nowdatetime = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute;
                        videoTbl.CoverImage = "/Files/VideoCoverImage/" + nowdatetime + Path.GetFileName(CoverImage.FileName);
                        CoverImage.SaveAs(Server.MapPath("~" + videoTbl.CoverImage));
                    }

                    videoTbl.ModifiedDate = DateTime.Now;
                    db.Entry(videoTbl).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Record Update Successfully";
                    return RedirectToAction("Index");
                }
                return View(videoTbl);
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
                int noOfRowUpdated = db.Database.ExecuteSqlCommand("Update VideoTbl set IsActive='0',ModifiedDate='" + datenow + "' where Id = " + id + "");
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
