using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.ViewModels.Account
{
    public class AbstractAccountViewModel
    {
        [Required(ErrorMessage = "Введите имя пользователя.")]
        [Display(Name = "Имя пользователя")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Имя пользователя должно быть от {2} до {1} символов", MinimumLength = 3)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль.")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Пароль должен быть от {2} до {1} символов", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
