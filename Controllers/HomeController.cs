using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eProject3_Vehicle_Showroom_Management.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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

                public ActionResult ProductDetail()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult ListProduct()
        {
            return View();
        }
    }
}