using Simple_CMS.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
