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
    public class AlbumController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();
        public ActionResult Index()
        {
            return View(db.AlbumTbls.Where(a=> a.IsActive == 1).ToList().OrderByDescending(a=> a.Id));
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,ImagePath,CreateDate,ModifiedDate,IsActive")] AlbumTbl albumTbl , HttpPostedFileBase ImagePath)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ImagePath != null)
                    {
                        string nowdatetime = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute;
                        albumTbl.ImagePath = "/Files/AlbumImages/" + nowdatetime + Path.GetFileName(ImagePath.FileName);
                        ImagePath.SaveAs(Server.MapPath("~" + albumTbl.ImagePath));
                        albumTbl.Id = (db.AlbumTbls.Select(x => (long?)x.Id).Max() ?? 0) + 1;
                        albumTbl.CreateDate = DateTime.Now;
                        albumTbl.IsActive = 1;
                        db.AlbumTbls.Add(albumTbl);
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
                return View(albumTbl);
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
            AlbumTbl albumTbl = db.AlbumTbls.Find(id);
            if (albumTbl == null)
            {
                return HttpNotFound();
            }
            return View(albumTbl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,ImagePath,CreateDate,ModifiedDate,IsActive")] AlbumTbl albumTbl , HttpPostedFileBase ImagePath)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ImagePath != null)
                    {
                        string nowdatetime = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute;
                        albumTbl.ImagePath = "/Files/AlbumImages/" + nowdatetime + Path.GetFileName(ImagePath.FileName);
                        ImagePath.SaveAs(Server.MapPath("~" + albumTbl.ImagePath));
                    }

                    albumTbl.ModifiedDate = DateTime.Now;
                    db.Entry(albumTbl).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Record Update Successfully";
                    return RedirectToAction("Index");
                }
                return View(albumTbl);
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
                int noOfRowUpdated = db.Database.ExecuteSqlCommand("Update AlbumTbl set IsActive='0',ModifiedDate='" + datenow + "' where Id = " + id + "");
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

          [HttpPost]
        public ActionResult ManageGallery([Bind(Include = "Id,AlbumID,ImagePath")] GalleryTbl galleryTbl, HttpPostedFileBase[] ImagePath)
        {
            if (ModelState.IsValid)
            {
                if (ImagePath != null)
                {
                    foreach (HttpPostedFileBase image in ImagePath)
                    {
                        string nowdatetime = DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute;
                        var InputFileName = Path.GetFileName(image.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Files/AlbumImages/") + nowdatetime + InputFileName);
                        string ImagePaths = "/Files/AlbumImages/" + nowdatetime + Path.GetFileName(image.FileName);
                        image.SaveAs(ServerSavePath);
                        long bmID = (db.GalleryTbls.Select(x => (long?)x.Id).Max() ?? 0) + 1;
                        db.Database.ExecuteSqlCommand("Insert into GalleryTbl values(" + bmID + "," + galleryTbl.AlbumID + ",'" + ImagePaths + "' ," + 1 + ")"); 
                    }
                    TempData["SuccessMessage"] = "Images Successfully Updated";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["FailMessage"] = "Please Select Atleast one image";
                    return RedirectToAction("Index");
                }
            }
            TempData["FailMessage"] = "Something goes wrong, Please try again";
            return RedirectToAction("Index");
        }

        [ActionName("PhotoDelete")]
        public ActionResult PhotoDelete(long id)
        {
            try
            {
                GalleryTbl imageTbl = db.GalleryTbls.Find(id);
                db.GalleryTbls.Remove(imageTbl);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Gallery Image Deleted Successfully";
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
