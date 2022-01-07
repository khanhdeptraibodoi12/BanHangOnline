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
    public class AccountController : Controller
    {
        menfashionEntities db = new menfashionEntities();
        // GET: Admin/Account

        //Nhân viên
        public ActionResult ManageAccount(int? page, string Search)
        {
            // Phân trang table
            int pageSize = 4;
            int pageNum = (page ?? 1);

            //Tim kiem
            if(Search != null)
            {
                var account = db.Members.OrderBy(m => m.dateOfJoin).Where(m => m.firstName.Contains(Search) || m.lastName.Contains(Search)).ToPagedList(pageNum, pageSize);
                return View(account);
            }
            else
            {
                var account = db.Members.OrderBy(m => m.dateOfJoin).Where(m => m.roleId != 3).ToPagedList(pageNum, pageSize);
                return View(account);
            }            
        }

        // Khách hàng
        public ActionResult MemberAccount(int? page, string Search)
        {
            // Phân trang table
            int pageSize = 4;
            int pageNum = (page ?? 1);

            //Tim kiem
            if (Search != null)
            {             
                var account = db.Members.OrderBy(m => m.dateOfJoin).Where(m => m.firstName.Contains(Search) || m.lastName.Contains(Search)).ToPagedList(pageNum, pageSize);
                return View(account);
            }
            else
            {
                var account = db.Members.OrderBy(m => m.dateOfJoin).Where(m => m.roleId == 3).ToPagedList(pageNum, pageSize);
                return View(account);
            }
        }

        public ActionResult Delete(string userName)
        {
            var del = db.Members.Where(m => m.userName == userName).SingleOrDefault();
            if (del != null)
            {
                db.Members.Remove(del);
                db.SaveChanges();
                return RedirectToAction("ManageAccount");
            }
            return View("ManageAccount");
        }

        public ActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(Member member)
        {           
            if (ModelState.IsValid)
            {
                var Check = db.Members.Where(m => m.userName == member.userName).SingleOrDefault();
                if(Check != null)
                {
                    ModelState.AddModelError("", "This account already exist");
                }
                else
                {
                    member.password = Encrytor.MD5Hash(member.password);
                    member.dateOfJoin = DateTime.Now;
                    member.roleId = 2;
                    member.status = true;
                    member.avatar = "~/Content/img/Avatar/Avatar.jpg";
                    db.Members.Add(member);
                    db.SaveChanges();
                    return RedirectToAction("Save");
                }
            }
            return View();
        }

        public ActionResult Edit(string userName)
        {
            var account = db.Members.Where(m => m.userName == userName).SingleOrDefault();
            return View(account);
        }

        [HttpPost]
        public ActionResult Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                var edit = db.Members.Where(m => m.userName == member.userName).SingleOrDefault();
                if (edit != null)
                {
                    edit.password = Encrytor.MD5Hash(member.password);
                    edit.firstName = member.firstName;
                    edit.lastName = member.lastName;
                    edit.email = member.email;
                    edit.birthday = member.birthday;
                    edit.identityNumber = member.identityNumber;
                    edit.phone = member.phone;
                    edit.dateOfJoin = member.dateOfJoin;
                    edit.address = member.address;
                    edit.avatar = "~/Content/img/avatar/avatar.jpg";
                    edit.status = true;
                    edit.roleId = 2;
                    db.SaveChanges();
                    return RedirectToAction("ManageAccount");
                }
            }
            return View(member);
        }

        public ActionResult Detail(string userName)
        {
            var account = db.Members.Where(m => m.userName == userName).SingleOrDefault();
            return View(account);
        }
    }
}