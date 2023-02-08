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
    public class NewsController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();

        public ActionResult Index()
        {
            return View(db.NewsTbls.Where(n=> n.IsActive == 1).ToList().OrderByDescending(n=> n.Id));
        }

        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,PublishDate,ImagePath,CreateDate,ModifiedDate,IsActive")] NewsTbl newsTbl , HttpPostedFileBase ImagePath)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ImagePath != null)
                    {
                        string nowdatetime = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute;
                        newsTbl.ImagePath = "/Files/NewsImages/" + nowdatetime + Path.GetFileName(ImagePath.FileName);
                        ImagePath.SaveAs(Server.MapPath("~" + newsTbl.ImagePath));
                        newsTbl.Id = (db.NewsTbls.Select(x => (long?)x.Id).Max() ?? 0) + 1;
                        newsTbl.CreateDate = DateTime.Now;
                        newsTbl.IsActive = 1;
                        db.NewsTbls.Add(newsTbl);
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
                return View(newsTbl);
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
            NewsTbl newsTbl = db.NewsTbls.Find(id);
            if (newsTbl == null)
            {
                return HttpNotFound();
            }
            return View(newsTbl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,PublishDate,ImagePath,CreateDate,ModifiedDate,IsActive")] NewsTbl newsTbl , HttpPostedFileBase ImagePath)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ImagePath != null)
                    {
                        string nowdatetime = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute;
                        newsTbl.ImagePath = "/Files/NewsImages/" + nowdatetime + Path.GetFileName(ImagePath.FileName);
                        ImagePath.SaveAs(Server.MapPath("~" + newsTbl.ImagePath));
                    }

                    newsTbl.ModifiedDate = DateTime.Now;
                    db.Entry(newsTbl).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Record Update Successfully";
                    return RedirectToAction("Index");
                }
                return View(newsTbl);
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
                int noOfRowUpdated = db.Database.ExecuteSqlCommand("Update NewsTbl set IsActive='0',ModifiedDate='" + datenow + "' where Id = " + id + "");
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
