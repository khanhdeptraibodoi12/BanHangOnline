using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Text.Models;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;

namespace Text.Areas.Admin.Controllers
{
    public class ChartController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        // GET: Admin/Chart
        public ActionResult Chart()
        {
            return View();
        }

        public ActionResult Revenue(int? page)
        {
            var revenue = db.vDoanhThuTheoNgays.ToList();
            return View(revenue);
        }

        public ActionResult Delete(string dateOrder)
        {
            var del = db.vDoanhThuTheoNgays.Where(m => m.dateOrder == dateOrder).SingleOrDefault();
            if (del != null)
            {
                db.vDoanhThuTheoNgays.Remove(del);
                db.SaveChanges();
                return RedirectToAction("Revenue");
            }
            return View("Revenue");
        }
    }
}