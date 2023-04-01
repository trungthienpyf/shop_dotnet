using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.UI;
using Shop_dotNet.Models;

namespace Shop_dotNet.Areas.Admin.Controllers
{
    [Authorize(Roles = "1")]
    public class OrdersController : Controller
    {
        private ShopEntities db = new ShopEntities();

        // GET: Admin/Orders
        public ActionResult Index(int? page,string q)
        {
            if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo LinkID mới có thể phân trang.

           
            var orders = db.orders.Include(o => o.customer).OrderByDescending(c=>c.id);
            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 5;
            if (q != null)
                orders = (IOrderedQueryable<order>)orders.Where(c => c.name_receive.Contains(q));
            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 5. Trả về các Link được phân trang theo kích thước và số trang.
            return View(orders.ToPagedList(pageNumber, pageSize));
           
          
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order_details= db.detail_orders.Where(c=>c.orders_id==id).Include(o=>o.order).Include(o => o.product).ToList();
            List<CartItem> list = new List<CartItem>();
          
            foreach (var item in order_details)
            {
                list.Add(new CartItem { product = db.products.Find(item.product_id), Quantity = Int32.Parse(item.quantity) });

            }

            return View(list);
        }
        public ActionResult UpdateStatus(int? type,int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }else if(type==1)
            {
                var order=db.orders.Find(id);
                order.status = type;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                var order = db.orders.Find(id);
                order.status = type;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
         

        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.customers, "id", "name");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,time,name_receive,phone_receive,address_receive,note,status,total_price,customer_id")] order order)
        {
            if (ModelState.IsValid)
            {
                db.orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.customers, "id", "name", order.customer_id);
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.customers, "id", "name", order.customer_id);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,time,name_receive,phone_receive,address_receive,note,status,total_price,customer_id")] order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.customers, "id", "name", order.customer_id);
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            order order = db.orders.Find(id);
            db.orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
