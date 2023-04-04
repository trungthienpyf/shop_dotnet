using Newtonsoft.Json;
using Shop_dotNet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop_dotNet.Areas.Admin.Controllers
{
    [Authorize(Roles = "1")]
    public class DashBoardController : Controller
    {
        Dictionary<string, int> monthNumbers = new Dictionary<string, int>
{
        {"January", 1},
        {"February", 2},
        {"March", 3},
        {"April", 4},
        {"May", 5},
        {"June", 6},
        {"July", 7},
        {"August", 8},
        {"September", 9},
        {"October", 10},
        {"November", 11},
        {"December", 12}
        };
        private ShopEntities db = new ShopEntities();
        // GET: Admin/DashBoard
        public ActionResult Index()
        {
            ViewBag.sum = db.orders.Sum(c => c.total_price);
            ViewBag.orders_new = db.orders.Where(c => c.status==0).Count();
            ViewBag.cus = db.customers.Where(c => c.role!=1).Count();
            ViewBag.orders_reject = db.orders.Where(c => c.status == 2).Count();


            int maxDate = 30;
            DateTime today = DateTime.Today;
            string lastMonthDate, lastMonth;
            int getDayLastMonth, startDayLastMonth, maxDayLastMonth;

            if (maxDate > today.Day)
            {
                getDayLastMonth = 30 - today.Day;
                lastMonthDate = today.AddMonths(-1).ToString("yyyy-MM-dd");
                lastMonth = today.AddMonths(-1).ToString("MM");
                maxDayLastMonth = DateTime.DaysInMonth(today.Year, today.AddMonths(-1).Month);
                startDayLastMonth = maxDayLastMonth - getDayLastMonth;
            }
            else
            {
                getDayLastMonth = 31 - today.Day;
                lastMonthDate = today.AddMonths(-1).ToString("yyyy-MM-dd");
                lastMonth = today.AddMonths(-1).ToString("MM");
                maxDayLastMonth = DateTime.DaysInMonth(today.Year, today.AddMonths(-1).Month);
                startDayLastMonth = maxDayLastMonth - getDayLastMonth;
            }

            Dictionary<string, float> dict = new Dictionary<string, float>();
            for (int i = startDayLastMonth; i <= maxDayLastMonth; i++)
            {
                string key = $"{i}-{lastMonth}";
                dict[key] = 0;
            }

            for (int i = 1; i <= today.Day; i++)
            {
                string key = $"{i}-{today.Month}";
                dict[key] = 0;
            }

            var result = db.orders
            .Where(o => o.status == 1)
             .GroupBy(o => SqlFunctions.DateName("day", o.time.Value) + "-" +SqlFunctions.DatePart("month", o.time.Value))
            .Select(g => new {
                TimeDate = g.Key,
                Sum = g.Sum(o => o.total_price)
            }).ToArray();
            foreach (var each in result)
            {
                if (dict.ContainsKey(each.TimeDate))
                {
                    dict[each.TimeDate] = (float)each.Sum;
                }
            }
            ViewBag.Arr1 = JsonConvert.SerializeObject(dict.Keys.ToArray()) ;
            ViewBag.Arr2 = JsonConvert.SerializeObject(dict.Values.ToArray());
            return View();
        }
    }
}