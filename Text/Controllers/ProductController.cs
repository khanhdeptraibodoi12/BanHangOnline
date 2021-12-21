using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Text.Models;
using PagedList;
using PagedList.Mvc;
namespace Text.Controllers
{  

    public class ProductController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        // GET: Product
        public ActionResult ShopCatalog(int? page, int? Id, string brand, string searching)
        {
            // bien quy dinh so san pham moi trang
            int pageSize = 1;
            // bien so trang
            int pageNum = (page ?? 1);
           
            if (searching != null)
            {
                ViewBag.searching = searching;
                var product = db.Products.OrderBy(m => m.dateCreate).Where(m => m.productName.Contains(searching)).ToPagedList(pageNum, pageSize);
                return View(product);
            }
            else
            { 
                if (Id == null && brand != null)
                {
                    ViewBag.ID = Id;
                    var product = db.Products.OrderBy(m => m.dateCreate).Include(p => p.ProductCategory).Where(m => m.brand == brand).ToPagedList(pageNum, pageSize);
                    return View(product);
                }
                else if(Id != null && brand==null)
                {
                    ViewBag.brand = brand;
                    var product = db.Products.OrderBy(m => m.dateCreate).Include(p => p.ProductCategory).Where(m => m.categoryId == Id).ToPagedList(pageNum, pageSize);
                    return View(product);
                }
                else
                {
                    var product = db.Products.OrderBy(m => m.dateCreate).Include(p => p.ProductCategory).ToPagedList(pageNum, pageSize);
                    return View(product);
                }
            }
        }
        public ActionResult ShopDetail(int Id)
        {
            var SanPham = db.Products.Where(m => m.productId == Id).SingleOrDefault();
            return View(SanPham);
        }


        public ActionResult Categories() // lay danh sach categories 
        {
            var List = db.ProductCategories.ToList();
            return View(List);
        }
        public ActionResult Brand() // lay danh sach nhan hang
        {
            List<string> brand = new List<string>();
            var listbarnd = db.Products.ToList();
            foreach(var i in db.Products)
            {
                if (!brand.Contains(i.brand.Trim()))
                    brand.Add(i.brand.Trim());
            }
            return View(brand);
        }
    }
}