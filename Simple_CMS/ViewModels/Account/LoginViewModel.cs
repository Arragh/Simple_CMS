using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.ViewModels.Account
{
    public class LoginViewModel : AbstractAccountViewModel
    {
        [Display(Name = "Запомнить")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
