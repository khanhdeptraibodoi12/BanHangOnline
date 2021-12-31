using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Text.Models;
using PagedList;
using PagedList.Mvc;

namespace Text.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        // GET: Admin/Product
        public ActionResult ManageProduct(int? page)
        {
            // Phân trang table
            int pageSize = 3;
            int pageNum = (page ?? 1);
           
            var product = db.Products.OrderBy(m=>m.dateCreate).ToPagedList(pageNum, pageSize);
            return View(product);
        }

        public ActionResult Save()
        {   
            return View();
        }
        public ActionResult Delete(int? Id)
        {
            var del = db.Products.Where(m => m.productId == Id).SingleOrDefault();
            if (del != null)
            {
                db.Products.Remove(del);
                db.SaveChanges();
                return RedirectToAction("ManageProduct");
            }
            return View("ManageProduct");
        }
    }
}