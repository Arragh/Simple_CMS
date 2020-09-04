using System.Collections.Generic;

namespace Simple_CMS.ViewModels.Gallery
{
    public class IndexViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<Models.Gallery.Gallery> Galleries { get; set; }
    }
}
