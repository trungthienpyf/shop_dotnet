using Shop_dotNet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Shop_dotNet.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        private ShopEntities db = new ShopEntities();

        public ActionResult Login()
        {
            if (Session["AdminID"] != null)
            {
                return RedirectToAction("Index", "Products");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(customer model)
        {
            using (db)
            {
                var IsValidUser = db.customers.Where(user => user.email ==
                     model.email.ToLower() && user.passsword == model.passsword).FirstOrDefault();
                
                if (IsValidUser!=null)
                {
                    Session["AdminID"] = IsValidUser.id.ToString();
                    Session["EmailAdmin"] = IsValidUser.email.ToString();
                    Session["NameAdmin"] = IsValidUser.name.ToString();

                    FormsAuthentication.SetAuthCookie(model.email, false);
                    return RedirectToAction("Index", "Products");
                }
                ModelState.AddModelError("", "Username hoặc Password không đúng");
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session.Remove("AdminID");
            Session.Remove("NameAdmin");
            Session.Remove("EmailAdmin");
            return RedirectToAction("Login");
        }
    }
}