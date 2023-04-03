using Shop_dotNet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop_dotNet.Controllers
{
    public class AccountController : Controller
    {
        private ShopEntities _dbContext = new ShopEntities();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
 
        public ActionResult Login(string email, string passsword)
        {
            if (ModelState.IsValid)
            {

                var data = _dbContext.customers.Where(s => s.email == email && s.passsword== passsword).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["Name"] = data.FirstOrDefault().name;
                    Session["Email"] = data.FirstOrDefault().email;
                    Session["idUser"] = data.FirstOrDefault().id;
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
       
        public ActionResult Register(customer _user)
        {

            if (_user.email == null)
            {
                ViewBag.emailError = "May nhap vao cho tao";
                return View();
            }
            if (_user.phone == null)
            {
                ViewBag.phoneError = "May nhap vao cho tao";
                return View();
            }

            if (ModelState.IsValid)
            {
                var check2 = _dbContext.customers.FirstOrDefault(s => s.email == _user.email);
                var check1 = _dbContext.customers.FirstOrDefault(s => s.phone == _user.phone);
                if (check1 == null)
                {
                    if (check2 == null)
                    {
                        customer c = new customer
                        {
                            name = _user.name,
                            email = _user.email,
                            phone = _user.phone,
                            passsword = _user.passsword,
                        };

                        _dbContext.customers.Add(c);
                        _dbContext.SaveChanges();
                        return RedirectToAction("Login"); ;
                    }
                    else
                    {
                        ViewBag.emailDuplicate = "Email trung r thang l";
                        return View();
                    }
                }
                else
                {
                    ViewBag.phoneDuplicate = "Phone trung r thang l";
                    return View();
                }

            }
            return View();


        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}