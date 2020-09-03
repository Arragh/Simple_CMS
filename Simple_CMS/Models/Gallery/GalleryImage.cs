using Simple_CMS.AbstractModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.Models.Gallery
{
    public class GalleryImage : AbstractImage
    {
        public Guid GalleryId { get; set; }
    }
}
