using Simple_CMS.Models.News;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.ViewModels.News
{
    public abstract class AbstractNewsViewModel
    {
        [Required(ErrorMessage = "Требуется ввести заголовок.")]
        [Display(Name = "Заголовок")]
        [StringLength(100, ErrorMessage = "Заголовок должен быть от {1} до {2} символов.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        public string NewsTitle { get; set; }

        [Required(ErrorMessage = "Требуется ввести содержание")]
        [Display(Name = "Содержание")]
        [StringLength(5000, ErrorMessage = "Содержание должно быть от {1} до {2} символов.", MinimumLength = 10)]
        [DataType(DataType.Text)]
        public string NewsBody { get; set; }

        [Display(Name = "Загрузить изображение")]
        public NewsImage NewsImage { get; set; }
    }
}
