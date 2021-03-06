﻿using System;
using System.Collections.Generic;

namespace Simple_CMS.Models.News
{
    public class News
    {
        public Guid Id { get; set; }
        public string NewsTitle { get; set; }
        public string NewsBody { get; set; }
        public DateTime NewsDate { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<NewsImage> NewsImages { get; set; }
    }
}
