using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.Models.Admin
{
    public class Settings
    {
        public Guid Id { get; set; }
        public string SettingsName { get; set; }

        #region News
        public int NewsImageSize { get; set; } // Объем загружаемой картинки в новости
        public int ImagesPerNews { get; set; } // Количество картинок в новости
        public int NewsPerPage { get; set; } // Количество новостей на страницу
        #endregion
    }
}
