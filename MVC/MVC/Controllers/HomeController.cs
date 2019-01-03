using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.ViewModels;
using WebApplication1.Models;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        Model m;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Forum()
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
    }
}