using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop_dotNet.Controllers
{
    public class PageController : Controller
    {
        // GET: Blog
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult CollectionPage()
        {
            return View();
        }
    }
}