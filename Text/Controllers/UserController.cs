using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Text.Models;
namespace Text.Controllers
{
    public class UserController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        // GET: User
        public ActionResult Login()
        {
            //.
            
            return View();
        }
        [HttpPost]


        public ActionResult Login(FormCollection form)
        {

            if (ModelState.IsValid)
            {
                var UserName = form["UserName"];
                var PassWord = form["PassWord"];
                PassWord = Encrytor.MD5Hash(PassWord);
                var User = db.Members.Where(model => model.userName == UserName && model.password == PassWord).SingleOrDefault();
                if (User != null)
                {
                    Session["UserLogin"] = User;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Isvalid UserName or PassWord ");
                }
            }
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Member member)
        {   
            if(ModelState.IsValid)
            {
                var Check =  db.Members.Where(m => m.userName ==member.userName).SingleOrDefault();
                if (Check != null)
                {
                    ModelState.AddModelError("", "Isvalid UserName or PassWord ");
                }
                else
                {
                    member.password = Encrytor.MD5Hash(member.password);
                    member.dateOfJoin = DateTime.Now;
                    member.roleId = 3;
                    member.status = true;
                    member.avatar = "~/Content/img/Avatar/Avatar.jpg"; 
                    db.Members.Add(member);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("UserLogin");
            return RedirectToAction("Index", "Home");
        }
    }
}