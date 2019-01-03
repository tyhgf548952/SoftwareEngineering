using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class forum
    {
        [Required]
        public string Search { get; set; }

        public string forum_name { get; set; }
        public List<Post> Post { get; set; }
    }
}