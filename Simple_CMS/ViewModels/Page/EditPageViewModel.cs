using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.ViewModels.Page
{
    public class EditPageViewModel
    {
        public Guid PageId { get; set; }

        [Required(ErrorMessage = "Требуется ввечсти заголовок")]
        [Display(Name = "Заголовок страницы")]
        [StringLength(100, ErrorMessage = "Заголовок должен быть от {1} до {2} символов.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        public string PageTitle { get; set; }

        [Required(ErrorMessage = "Требуется ввести содержание")]
        [Display(Name = "Содержание страницы")]
        [StringLength(50000, ErrorMessage = "Содержание должно быть от {1} до {2} символов.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        public string PageBody { get; set; }
    }
}
