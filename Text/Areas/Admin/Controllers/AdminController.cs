using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Text.Models;
using System.Web.Mvc;

namespace Text.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        // GET: Admin/Home
        public ActionResult Index(int? Id)
        {
            var roleName = db.Roles.Where(m => m.roleId == Id).SingleOrDefault();
            ViewBag.RoleName = roleName.roleName;
            return View();
        }


    }
}