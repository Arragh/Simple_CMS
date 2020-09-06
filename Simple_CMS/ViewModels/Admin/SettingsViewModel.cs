using System.ComponentModel.DataAnnotations;

namespace Simple_CMS.ViewModels.Admin
{
    public class SettingsViewModel
    {
        [Required]
        [Display(Name = "Объем загружаемой картинки")]
        public int NewsImageSize { get; set; }

        [Required]
        [Display(Name = "Количество картинок на одну запись")]
        public int ImagesPerNews { get; set; }

        [Required]
        [Display(Name = "Количество записей на страницу")]
        public int NewsPerPage { get; set; }
    }
}
