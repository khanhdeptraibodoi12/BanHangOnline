using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Text.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ShopCatalog()
        {
            return View();
        }
        public ActionResult ShopDetail()
        {
            return View();
        }


    }
}