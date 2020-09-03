using Simple_CMS.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.ViewModels.News
{
    public class EditNewsViewModel : AbstractNewsViewModel
    {
        public Guid NewsId { get; set; }
        public DateTime NewsDate { get; set; }

        public string UserName { get; set; }

        //[Display(Name = "Загрузить изображение")]
        //public NewsImage NewsImage { get; set; }

        public List<NewsImage> NewsImages { get; set; }

        public int ImagesCount { get; set; }
    }
}
