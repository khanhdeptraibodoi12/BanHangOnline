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
    public class InvoinceController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        // GET: Admin/Invoince
        public ActionResult ManageInvoince(int? page)
        {
            // Phân trang table
            int pageSize = 3;
            int pageNum = (page ?? 1);

            var invoince = db.Invoinces.OrderBy(i => i.dateOrder).Include(i => i.InvoinceDetails).ToPagedList(pageNum, pageSize);
            return View(invoince);
        }

        public ActionResult Delete(string invoinceNo)
        {
            var del = db.Invoinces.Include(m => m.InvoinceDetails).Where(m => m.invoinceNo == invoinceNo).SingleOrDefault();
            if (del != null)
            {
                db.Invoinces.Remove(del);              
                db.SaveChanges();
                return RedirectToAction("ManageInvoince");
            }
            return View("ManageInvoince");
        }

        public ActionResult Detail(string invoinceNo)
        {
            var invoince = db.InvoinceDetails.Include(m => m.Product).Include(m => m.Invoince).Where(m => m.invoinceNo == invoinceNo).ToList();
            return View(invoince);
        }
    }
}