 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class ForumController : Controller
    {
        public ActionResult SoftwareEnginering()
        {
            return View();
        }
        public ActionResult OperatingSystem()
        {
            return View();
        }
        public ActionResult Post()
        {
            MyView model = new MyView();
            return View();
        }
        [HttpPost]
        public ActionResult Post(MyView model)
        {
            return View(model);
        }

        public ActionResult Search()
        {
            MyView model = new MyView();
            return View();
        }
        [HttpPost]
        public ActionResult Search(MyView model)
        {
            return View(model);
        }
    }
}