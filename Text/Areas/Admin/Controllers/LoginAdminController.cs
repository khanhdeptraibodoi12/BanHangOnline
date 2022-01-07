using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Text.Models;

namespace Text.Areas.Admin.Controllers
{
    public class LoginAdminController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        // GET: Admin/LoginAdmin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            if(ModelState.IsValid)
            {
                var userName = form["UserName"];
                var passWord = form["PassWord"];
                passWord = Encrytor.MD5Hash(passWord);
                var account = db.Members.Where(model => model.userName == userName && model.password == passWord && (model.roleId == 1 || model.roleId == 2)).SingleOrDefault();
                if(account != null)
                {
                    Session["UserLogin"] = account;
                    return RedirectToAction("Index", "Admin" ,new {Id = account.roleId });
                }
                else
                {
                    ModelState.AddModelError("", "Isvalid UserName or PassWord ");
                }
            }
            return View();
        }

        public ActionResult logOut()
        {
            Session.Remove("UserLogin");
            return RedirectToAction("Index");
        }
    }
}