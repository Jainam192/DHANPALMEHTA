using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PoliticalPortfolio.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private PoliticalDBEntities db = new PoliticalDBEntities();
        public ActionResult Index()
        {
            if (Session["AdminID"] != null)
            {
                return RedirectToAction("Index", "DashBoard");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AdminLoginTbl objAdmin)
        {
            if (ModelState.IsValid)
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] ePass;
                UTF8Encoding encoder = new UTF8Encoding();
                ePass = md5Hasher.ComputeHash(encoder.GetBytes(objAdmin.Password));
                string Password = Convert.ToBase64String(ePass);
                var obj = db.AdminLoginTbls.Where(a => a.UserName.Equals(objAdmin.UserName) && a.Password.Equals(Password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["AdminID"] = obj.Id.ToString();
                    Session["AdminName"] = obj.UserName.ToString();
                    return RedirectToAction("Index", "DashBoard");
                }
            }
            ViewBag.Message = "Invalid User Name Password";
            return View(objAdmin);
        }
    }
}