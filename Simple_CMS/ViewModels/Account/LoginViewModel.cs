using Simple_CMS.AbstractModels.ViewModels.Account;
using System.ComponentModel.DataAnnotations;

namespace Simple_CMS.ViewModels.Account
{
    public class LoginViewModel : AbstractAccountViewModel
    {
        [Display(Name = "Запомнить")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
