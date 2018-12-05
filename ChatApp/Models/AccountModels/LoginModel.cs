using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatApp.Models.AccountModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Login")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}