using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.ViewModels;
//using WebApplication1.Models;
namespace MVC.Controllers
{
    public class UserController : Controller
    {
        public ActionResult UserPage()
        {
            MyView model = new MyView();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserPage(MyView model)
        {
            return View(model);
        }

        public ActionResult UserPage_1()
        {
            MyView model = new MyView();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserPage_1(MyView model)
        {
            return View(model);
        }
    }
}