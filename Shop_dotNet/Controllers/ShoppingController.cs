using Shop_dotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Shop_dotNet.Controllers
{
    public class ShoppingController : Controller
    {
        // GET: Shopping
        private ShopEntities db = new ShopEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CheckOut()
        {
            return View();
        }
            
        
        // GET: Shopping
        

        public ActionResult Buy(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["cart"] == null)
            {

                List<CartItem> cart = new List<CartItem>();
                cart.Add(new CartItem { product = db.products.Find(id), Quantity = 1 });
                Session["cart"] = cart;

            }
            else
            {
                List<CartItem> cart = (List<CartItem>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { product = db.products.Find(id), Quantity = 1 });
                }
                Session["cart"] = cart;
            }

            return RedirectToAction("Index");
        }

        public ActionResult UpdateSL(int type, int id)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];
            if (type == 1)
            {                             
               
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }                
                Session["cart"] = cart;
                return RedirectToAction("Index");
            }
            else
            {
                int index = isExist(id);
                if (cart[index].Quantity == 1)
                {
                    cart.RemoveAt(index);
                    Session["cart"] = cart;
                }
                else
                {
                    cart[index].Quantity--;
                    Session["cart"] = cart;
                }

                return RedirectToAction("Index");
            }
            

        }
        private int isExist(int id)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].product.id.Equals(id))
                    return i;
            return -1;
        }

        public ActionResult Remove(int id)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];
            int index = isExist(id);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Index");
        }
        
        public ActionResult Payment()
        {

            return View();
        }
    }
}