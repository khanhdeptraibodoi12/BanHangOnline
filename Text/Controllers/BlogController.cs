using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Text.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult BlogList()
        {
            return View();
        }
        public ActionResult BlogDetail()
        {
            return View();
        }
    }
}