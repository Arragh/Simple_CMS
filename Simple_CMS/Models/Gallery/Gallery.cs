using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.Models.Gallery
{
    public class Gallery
    {
        public Guid Id { get; set; }
        public string GalleryTitle { get; set; }
        public string GalleryDescription { get; set; }
        public DateTime GalleryDate { get; set; }
        public string UserName { get; set; }
        public string GalleryPreviewImage { get; set; }
        public virtual ICollection<GalleryImage> GalleryImages { get; set; }
    }
}
