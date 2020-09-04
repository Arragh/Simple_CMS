using Simple_CMS.Models.News;
using System.Collections.Generic;

namespace Simple_CMS.ViewModels.News
{
    public class IndexViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<Models.News.News> News { get; set; }
        public List<NewsImage> NewsImages { get; set; }
    }
}
