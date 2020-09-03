using Simple_CMS.AbstractModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.Models.News
{
    public class NewsImage : AbstractImage
    {
        public Guid NewsId { get; set; }
    }
}
