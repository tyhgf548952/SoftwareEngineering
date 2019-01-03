using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MVC.ViewModels
{
    public class Post
    {
        public string Author { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        public string Time { get; set; }

        public List<string> Comments { get; set; }
    }
}
