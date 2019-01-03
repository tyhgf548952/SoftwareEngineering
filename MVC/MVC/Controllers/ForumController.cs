 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.ViewModels;
using WebApplication1.Models;

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
        public ActionResult Search(MyView model,Model m)
        { 
            Board board = m.searchBoard(model.forum.forum_name);
            model.forum.forum_name = board.Name;
            return View(model);
        }
    }
}