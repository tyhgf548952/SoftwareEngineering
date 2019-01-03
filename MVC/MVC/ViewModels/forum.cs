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
    }
}