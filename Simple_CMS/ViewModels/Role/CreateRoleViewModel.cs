using System.ComponentModel.DataAnnotations;

namespace Simple_CMS.ViewModels.Roles
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Требуется ввести название роли.")]
        [Display(Name = "Введите название роли")]
        [DataType(DataType.Text)]
        [StringLength(30, ErrorMessage = "Название роли должно содержать от {2} до {1} символов.", MinimumLength = 4)]
        public string RoleName { get; set; }
    }
}
