using Shop_dotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop_dotNet.Controllers
{
    public class HomeController : Controller
    {
        ShopEntities db = new ShopEntities();
        public ActionResult Index(string searchString)
        {
            var _products = from p in db.products select p; 
            

            if(!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                _products = _products.Where(p => p.name.ToLower().Contains(searchString.ToLower()));
                return View(_products.ToList());
            }


            return View(db.products.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult test()
        {
            return View();
        }
    }
}