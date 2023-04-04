using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
using Shop_dotNet.Models;
using static System.Net.WebRequestMethods;

namespace Shop_dotNet.Areas.Admin.Controllers
{
    [Authorize(Roles = "1")]
    public class ProductsController : Controller
    {
        private ShopEntities db = new ShopEntities();

        // GET: Admin/products
        public ActionResult Index(int? page,string q)
        {
           
            if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo LinkID mới có thể phân trang.
          
            var products = db.products.Include(p => p.category).OrderBy(p=>p.id);
            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 5;
            if (q != null)
                products = (IOrderedQueryable<product>)products.Where(c => c.name.Contains(q));
            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 5. Trả về các Link được phân trang theo kích thước và số trang.
            return View(products.ToPagedList(pageNumber, pageSize));
            
        }

        // GET: Admin/products/Details/5
        
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/products/Create
   
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.categories, "id", "name");
          
            return View();
        }

        // POST: Admin/products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,description,img,price,manufacturers_id,category_id")] product product)
        {
            if (ModelState.IsValid)
            {
                var guid = Guid.NewGuid().ToString();
                HttpPostedFileBase file = Request.Files["upload"];
                if (file != null && file.ContentLength > 0)
                {

                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Areas/Admin/Assets/products"), guid+fileName);

                    file.SaveAs(path);
                    product.img = guid+fileName;
                }
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.categories, "id", "name", product.category_id);
          //  ViewBag.manufacturers_id = new SelectList(db.manufacturers, "id", "name", product.manufacturers_id);
            return View(product);
        }

        // GET: Admin/products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.categories, "id", "name", product.category_id);
        //    ViewBag.manufacturers_id = new SelectList(db.manufacturers, "id", "name", product.manufacturers_id);
            return View(product);
        }

        // POST: Admin/products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,img,price,manufacturers_id,category_id")] product product)
        {
            if (ModelState.IsValid)
            {
                var guid = Guid.NewGuid().ToString();
                string oldimage = Request.Form["oldimage"];
                HttpPostedFileBase file = Request.Files["upload"];
                if (file != null && file.ContentLength > 0)
                {

                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Areas/Admin/Assets/products"), guid + fileName);
                    product getProductRomoveImg = db.products.First(s => s.id == product.id);
                    if (getProductRomoveImg.img != null)
                    {
                        string pathRemove = Path.Combine(Server.MapPath("~/Images"), getProductRomoveImg.img);
                        if (System.IO.File.Exists(pathRemove))
                        {

                            System.IO.File.Delete(pathRemove);
                        }
                    }
                    file.SaveAs(path);
                    product.img = guid + fileName;
                }
                else
                {
                    product.img = oldimage;

                }
                db.products.AddOrUpdate(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.category_id = new SelectList(db.categories, "id", "name", product.category_id);
         //   ViewBag.manufacturers_id = new SelectList(db.manufacturers, "id", "name", product.manufacturers_id);
            return View(product);
        }

        // GET: Admin/products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
           //return Content("<script language='javascript' type='text/javascript'>alert('ABC');</script>");

            return View(product);
        }

        // POST: Admin/products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            if (db.detail_orders.Any(d => d.product_id == id)){
                ViewBag.ErrorMessage = "Khong the xoa!!";
                return View(product);
            }
            if (product.img != null) { 
              
            string pathRemove = Path.Combine(Server.MapPath("~/Areas/Admin/Assets/products"), product.img);
            if (System.IO.File.Exists(pathRemove))
            {
                System.IO.File.Delete(pathRemove);
            }
            }
            db.products.Remove(product);
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
