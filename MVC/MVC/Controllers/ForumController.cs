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
    }
}