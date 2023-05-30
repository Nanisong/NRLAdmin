using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NRLAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "NRLAdmin.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "NRL contact.";

            return View();
        }
    }
}