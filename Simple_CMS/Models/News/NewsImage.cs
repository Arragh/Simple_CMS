using Simple_CMS.AbstractModels.Models;
using System;

namespace Simple_CMS.Models.News
{
    public class NewsImage : AbstractImage
    {
        public Guid NewsId { get; set; }
    }
}
