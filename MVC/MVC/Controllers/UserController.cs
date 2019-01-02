using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        public ActionResult UserPage()
        {
            User model = new User();
            return View();
        }

        [HttpPost]
        public ActionResult UserPage(User model)
        {
            return View(model);
        }
    }
}