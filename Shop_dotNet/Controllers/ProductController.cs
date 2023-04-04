using Shop_dotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlClient;
using System.CodeDom;

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
            var products = _dbContext.products;
            ViewBag.products=products;
            //    ViewBag.manufacturers_id = new SelectList(db.manufacturers, "id", "name", product.manufacturers_id);
            return View(product);
          
        }
        public ActionResult Catelogy(int? catelogy, string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            var products = _dbContext.products.Include(p => p.category);
            if (catelogy==3)
            {
                products = products.Where(p => p.category.id == catelogy);
                return View(products.ToList());
            }
            if (catelogy == 4)
            {
                products = products.Where(p => p.category.id == catelogy);
                return View(products.ToList());
            }
            if (catelogy == 5)
            {
                products = products.Where(p => p.category.id == catelogy);
                return View(products.ToList());
            }
            switch (sortOrder)
            {
                case "name_desc":
                    {
                        products = products.OrderByDescending(s => s.name);
                        break;
                    }
                case "Price":
                    {
                        products = products.OrderBy(s => s.price);
                        break;
                    }
                case "price_desc":
                        {
                            products = products.OrderByDescending(s => s.price);
                            break;
                        }
                default :
                    {
                        products = products.OrderBy(s => s.name);
                        break;
                    }
            }
            return View(products.ToList());
        }
       
    }
}