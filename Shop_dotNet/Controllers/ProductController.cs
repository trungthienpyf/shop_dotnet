using Shop_dotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Shop_dotNet.Controllers
{
    public class ProductController : Controller
    {
        private ShopEntities _dbContext = new ShopEntities();
        // GET: Product
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult ProductAccordion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = _dbContext.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
          
            //    ViewBag.manufacturers_id = new SelectList(db.manufacturers, "id", "name", product.manufacturers_id);
            return View(product);
          
        }
        public ActionResult Catelogy()
        {
            return View();
        }
       
    }
}