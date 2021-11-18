using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Text.Controllers
{
    public class HandlerCartController : Controller
    {
        // GET: HandlerCart
        public ActionResult Cart()
        {
            return View();
        }
        public ActionResult Checkout()
        {
            return View();
        }
    }
}