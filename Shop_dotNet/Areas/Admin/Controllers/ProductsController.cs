﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop_dotNet.Models;
using static System.Net.WebRequestMethods;

namespace Shop_dotNet.Areas.Admin.Controllers
{
    [Authorize(Roles = "1")]
    public class ProductsController : Controller
    {
        private ShopEntities db = new ShopEntities();

        // GET: Admin/products
        public ActionResult Index()
        {
            var products = db.products.Include(p => p.category);
            return View(products.ToList());
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
            return View(product);
        }

        // POST: Admin/products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);

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