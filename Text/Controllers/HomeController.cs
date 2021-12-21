using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Text.Models;



namespace Text.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        menfashionEntities db = new menfashionEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult MyAccount()
        {

            return View();
        }

        [HttpGet] // change account 

        public ActionResult ChangeAccount(string username)
        {
            var member = db.Members.Find(username);
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAccount(Member member , FormCollection formCollection)
        {
            var CurrentPassword = formCollection["Current"]; // Mật khẩu hiện tại
            var NewPassword = formCollection["new"]; // Mật khẩu mới
            var ConfirmPassword = formCollection["confirm"];//mat khau confirm
            CurrentPassword = Encrytor.MD5Hash(CurrentPassword);
            NewPassword = Encrytor.MD5Hash(NewPassword);
            ConfirmPassword = Encrytor.MD5Hash(ConfirmPassword);

            var check = db.Members.Where(model => model.password == CurrentPassword && model.userName == member.userName).FirstOrDefault();
            if (check != null)
            {
                if (NewPassword == ConfirmPassword)
                {
                    check.password = NewPassword;
                    db.SaveChanges();
                    TempData["msgChangePassword"] = "Successfully change password!";
                    return RedirectToAction("index");
                }
                else
                {
                    ModelState.AddModelError("", "password must match!");
                    return View(member);
                }
            }
            else
            {
                ModelState.AddModelError("", "Incorrect your password!");
                return View(member);
            }

        }

    }
}
