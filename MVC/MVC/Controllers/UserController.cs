using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.ViewModels;
using WebApplication1.Models;
namespace MVC.Controllers
{
    public class UserController : Controller
    {
        Model m;
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
    }
}