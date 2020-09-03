﻿using Simple_CMS.Models.Gallery;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.ViewModels.Gallery
{
    public class ViewGalleryViewModel
    {
        public Guid GalleryId { get; set; }

        public string GalleryTitle { get; set; }
        public string GalleryDescription { get; set; }
        public DateTime GalleryDate { get; set; }
        public string UserName { get; set; }

        [Display(Name = "Загрузить изображение")]
        public GalleryImage GalleryImage { get; set; }
        public List<GalleryImage> GalleryImages { get; set; }
        public int ImagesCount { get; set; }
    }
}
