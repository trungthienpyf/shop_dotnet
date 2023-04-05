using Shop_dotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace Shop_dotNet.Controllers
{
    public class OrderController : Controller
    {
        
        private ShopEntities db = new ShopEntities();
        // GET: Order
        public ActionResult Index()
        {
            if (Session["Email"] == null || Session["Email"].ToString() == "")
                return RedirectToAction("Login", "Account");
            else
            {
                int id = (int) Session["idUser"];
                
                return View(db.orders.Where(c => c.customer_id == id ));
            }

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order_details = db.detail_orders.Where(c => c.orders_id == id).Include(o => o.order).Include(o => o.product).ToList();
            List<CartItem> list = new List<CartItem>();

            foreach (var item in order_details)
            {
                list.Add(new CartItem { product = db.products.Find(item.product_id), Quantity = Int32.Parse(item.quantity) });

            }

            return View(list);
        }
    }
}