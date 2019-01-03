using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class User
    {
        public string Search { get; set; }

        [DisplayName("帳號")]
        [Required]
        public string Account { get; set; }

        [DisplayName("密碼")]
        [Required]
        public string Password { get; set; }

        public List<string> friend { get; set; } 
    }
}