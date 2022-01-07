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
        public ActionResult ManageProduct(int? page, string Search)
        {
            // Phân trang table
            // Quy dinh so luong moi trang
            int pageSize = 3;
            // so trang
            int pageNum = (page ?? 1);
            // Tim kiem
            if (Search != null)
            {
                ViewBag.Search = Search;
                var product = db.Products.OrderBy(m => m.dateCreate).Where(m => m.productName.Contains(Search)).ToPagedList(pageNum, pageSize);
                return View(product);
            }
            else
            {
                var product = db.Products.OrderBy(m => m.dateCreate).ToPagedList(pageNum, pageSize);
                return View(product);
            }
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


        public ActionResult Save()
        {
            //Lay item vao dropdownlist
            ViewBag.CategoryId = new SelectList(db.ProductCategories, "categoryId", "categoryName");
            return View();
        }

        [HttpPost]
        public ActionResult Save(HttpPostedFileBase image ,Product product)
        {
            Member member = (Member)Session["UserLogin"];
            if(ModelState.IsValid)
            {   
                var Check = db.Products.Where(p => p.productName == product.productName).SingleOrDefault();
                var pic = db.Products.Where(p => p.image == product.image).SingleOrDefault();              
                if (Check != null)
                {
                    ModelState.AddModelError("", "This product is already exist");
                }
                else
                {
                    //extract only the filename
                    string imageName = System.IO.Path.GetFileName(image.FileName);
                    //store the file in folder
                    string physicalPath = "~/Content/img/product/" + imageName;
                    //Delete image exist
                    if(pic != null)
                    {
                        System.IO.File.Delete(Server.MapPath(physicalPath));
                        image.SaveAs(Server.MapPath(physicalPath));
                    }
                    else
                    {
                        image.SaveAs(Server.MapPath(physicalPath));
                    }
                    //Add all field data
                    product.dateCreate = DateTime.Now;
                    product.status = true;
                    product.userName = member.userName;
                    product.image = physicalPath;
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Save");
                }
            }
            ViewBag.CategoryId = new SelectList(db.ProductCategories, "categoryId", "categoryName", product.categoryId);
            return View();
        }

        public ActionResult Edit(int? Id)
        {
            var find = db.Products.Find(Id);            
            ViewBag.CategoryId = new SelectList(db.ProductCategories, "categoryId", "categoryName", find.categoryId);

            var product = db.Products.Where(p => p.productId == Id).SingleOrDefault();
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase image, Product product)
        {
            Member member = (Member)Session["UserLogin"];
            if (ModelState.IsValid)
            {
                var edit = db.Products.Where(p => p.productId == product.productId).SingleOrDefault();
                if (edit != null)
                {
                    //extract only the filename
                    string imageName = System.IO.Path.GetFileName(image.FileName);
                    //store the file in folder
                    string physicalPath = "~/Content/img/product/" + imageName;
                    image.SaveAs(Server.MapPath(physicalPath));
                    edit.image = physicalPath;

                    edit.productName = product.productName;
                    edit.price = product.price;
                    edit.discount = product.discount;
                    edit.description = product.description;
                    edit.quanlity = product.quanlity;
                    edit.brand = product.brand;
                    edit.dateCreate = DateTime.Now;
                    edit.description = product.description;
                    edit.userName = member.userName;
                    db.SaveChanges();
                    return RedirectToAction("ManageProduct");
                }                          
            }
            ViewBag.CategoryId = new SelectList(db.ProductCategories, "categoryId", "categoryName", product.categoryId);
            return View(product);
        }

        public ActionResult Detail(int? Id)
        {
            var product = db.Products.Where(p => p.productId == Id).SingleOrDefault();
            return View(product);
        }
                
    }
}