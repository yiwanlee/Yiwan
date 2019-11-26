using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RowversionTest.DAL;

namespace RowversionTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new QebbDBContext())
            {
                var d = db.GamDogluckyrec.OrderBy(q => q.QeIndex).Take(10).ToList();
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(d));
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}