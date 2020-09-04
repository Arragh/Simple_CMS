using Simple_CMS.AbstractModels.Models;
using System;

namespace Simple_CMS.Models.Gallery
{
    public class GalleryImage : AbstractImage
    {
        public Guid GalleryId { get; set; }
    }
}
