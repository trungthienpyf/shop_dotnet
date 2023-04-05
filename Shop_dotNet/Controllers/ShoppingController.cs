using System.Text.Json;
using Shop_dotNet.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Threading.Tasks;

namespace Shop_dotNet.Controllers
{
    
    public class ShoppingController : Controller
    {
        // GET: Shopping
        private ShopEntities db = new ShopEntities();
        string ShoppingCartId { get; set; }
        public ActionResult Index()
        {
            return View();
        }



        // GET: Shopping


        public ActionResult Buy(int id,string size)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["cart"] == null)
            {

                List<CartItem> cart = new List<CartItem>();
                cart.Add(new CartItem { product = db.products.Find(id), Quantity = 1,   size=size });
                Session["cart"] = cart;

            }
            else
            {
                List<CartItem> cart = (List<CartItem>)Session["cart"];
                int index = isExistAndSize(id,size);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { product = db.products.Find(id), Quantity = 1,size=size });
                }
                Session["cart"] = cart;
            }

            return RedirectToAction("Index");
        }

        public ActionResult UpdateSL(int type, int id,string size)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];
            if (type == 1)
            {

                int index = isExistAndSize(id,size);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                Session["cart"] = cart;
                return RedirectToAction("Index");
            }
            else
            {
                int index = isExistAndSize(id, size);
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
                if (cart[i].product.id.Equals(id) )
                    return i;
            return -1;
        }
        private int isExistAndSize(int id, string size)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].product.id.Equals(id) && cart[i].size == size)
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
        public List<CartItem> Laygiohang()

        {
            List<CartItem> dsGiohang = Session["Cart"] as List<CartItem>;
            if (dsGiohang == null)
            {
                dsGiohang = new List<CartItem>();
                Session["Cart"] = dsGiohang;
            }
            return dsGiohang;
        }
        [HttpGet]
        public ActionResult CheckOut()
        {
            //Kiem tra dang nhap
            if (Session["Email"] == null || Session["Email"].ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }
        /*[HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            //Them Don hang
            order ddh = new order();
            //customer kh = (customer)Session["Taikhoan"];
            var xuat = db.customers.Find();
            List<CartItem> gh = Laygiohang();
            ddh.id = xuat.id;
            if (collection["Ngaygiao"].Equals(""))
            {
                DateTime aDateTime = DateTime.Now;
                DateTime newTime = aDateTime.AddDays(7);
            }
            else
            {
                var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["Ngaygiao"]);
            }
            ddh.status = 0;
            db.orders.Add(ddh);
            db.SaveChanges();
            foreach (var item in gh)
            {
                //product giay = db.products.Single(n => n.id == item.iMAGIAY);
                //if ( >= item.iSOLUONG)
                //{
                //CTDONDATHANG ctdh = new CTDONDATHANG();
                //ctdh.MADH = ddh.MADH;
                //ctdh.MAGIAY = item.iMAGIAY;
                //ctdh.SOLUONG = item.iSOLUONG;
                //ctdh.DONGIA = (int)item.dDONGIA;
                //data.CTDONDATHANGs.InsertOnSubmit(ctdh);
                //giay.SOLUONG = giay.SOLUONG - item.iSOLUONG;
                db.SaveChanges();
                Session["Giohang"] = null;
               /// }
               // else
               // {
               //     return RedirectToAction("ThongBao", "Giohang");
               // }

            }
            return RedirectToAction("Xacnhandonhang", "Giohang");

        }*/
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel order)
        {
            int orderTotal = 0;
            List<CartItem> dsGiohang = (List < CartItem >) Session["Cart"] ;

            var addOrder = new order()
            {
                name_receive = order.name_receive,
                phone_receive = order.phone_receive,
                address_receive = order.address_receive,
                note = order.Note,
                customer_id = (int)Session["idUser"],
                status = 0,
                total_price = order.price,
                time = DateTime.Now,
            };
            db.orders.Add(addOrder);
            db.SaveChanges();
            foreach (var item in dsGiohang)
            {
                var orderDetail = new detail_orders
                {
                    product_id = item.product.id,
                    orders_id = addOrder.id,                   
                    quantity = item.Quantity.ToString(),
                    size= item.size
                };
                orderTotal += (int)(item.Quantity * item.product.price);

                db.detail_orders.Add(orderDetail);
                db.SaveChanges();
            }
            
            // Set the order's total to the orderTotal count
            //order.total_price = orderTotal;
            return RedirectToAction("Xacnhandonhang", "Shopping");
        }
       


        public ActionResult Xacnhandonhang()
        {
            return View();
        }

        public  String getSignature(String text, String key)
        {
            // change according to your needs, an UTF8Encoding
            // could be more suitable in certain situations
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}