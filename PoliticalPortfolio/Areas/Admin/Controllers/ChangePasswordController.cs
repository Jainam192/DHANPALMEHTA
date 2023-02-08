using PoliticalPortfolio.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PoliticalPortfolio.Areas.Admin.Controllers
{
    [AdminSessionCheck]
    public class ChangePasswordController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();
        public ActionResult Index()
        {
            return View();
        }
        [AdminSessionCheck]
        [HttpPost]
        public ActionResult Index(ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] ePass;
                UTF8Encoding encoder = new UTF8Encoding();
                ePass = md5Hasher.ComputeHash(encoder.GetBytes(changePassword.OldPassword));
                String oldPassword = Convert.ToBase64String(ePass);
                long userid = long.Parse(Session["AdminId"].ToString());
                var obj = db.AdminLoginTbls.Where(a => a.Password.Equals(oldPassword) && a.Id == userid).FirstOrDefault();
                if (obj != null)
                {
                    ePass = md5Hasher.ComputeHash(encoder.GetBytes(changePassword.NewPassword));
                    string newPassword = Convert.ToBase64String(ePass);
                    int noOfRowUpdated = db.Database.ExecuteSqlCommand("Update AdminLoginTbl set Password='" + newPassword + "' where Id = " + userid + "");
                    if (noOfRowUpdated == 0)
                    {
                        TempData["FailMessage"] = "Something goes wrong, Please try again";
                        return RedirectToAction("Index", "ChangePassword");
                    }
                    TempData["SuccessMessage"] = "Password Updated successfully";
                    return RedirectToAction("Index", "ChangePassword");
                }
                else
                {
                    TempData["FailMessage"] = "Old Password Not Matched";
                    return RedirectToAction("Index", "ChangePassword");
                }
            }
            TempData["FailMessage"] = "Something goes wrong, Please try again";
            return RedirectToAction("Index", "ChangePassword");
        }
    }
}