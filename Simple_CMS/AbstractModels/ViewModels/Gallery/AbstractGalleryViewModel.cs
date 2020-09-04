using System.ComponentModel.DataAnnotations;

namespace Simple_CMS.AbstractModels.ViewModels.Gallery
{
    public abstract class AbstractGalleryViewModel
    {
        [Required(ErrorMessage = "Введите название.")]
        [Display(Name = "Заголовок")]
        [StringLength(100, ErrorMessage = "Заголовок должен быть от {2} до {1} символов.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        public string GalleryTitle { get; set; }

        [Required(ErrorMessage = "Введите краткое описание.")]
        [Display(Name = "Краткое описание")]
        [StringLength(1000, ErrorMessage = "Описание должно быть от {2} до {1} символов.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        public string GalleryDescription { get; set; }

        [Display(Name = "Превью-Картинка")]
        public string GalleryPreviewImage { get; set; }
    }
}
