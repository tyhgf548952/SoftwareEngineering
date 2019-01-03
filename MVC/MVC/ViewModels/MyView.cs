using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class MyView
    {
        public forum forum { get; set; }
        public Post Post { get; set; }
        public User User { get; set; } 
    }
}