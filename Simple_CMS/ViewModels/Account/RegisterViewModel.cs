using Simple_CMS.AbstractModels.ViewModels.Account;
using System.ComponentModel.DataAnnotations;

namespace Simple_CMS.ViewModels.Account
{
    public class RegisterViewModel : AbstractAccountViewModel
    {
        [Required(ErrorMessage = "Введите адрес Email.")]
        [Display(Name = "Адрес Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Введите корректный адрес Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Повторите пароль.")]
        [Display(Name = "Повторите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string PasswordConfirm { get; set; }
    }
}
