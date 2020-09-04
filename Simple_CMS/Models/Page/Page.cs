using System;

namespace Simple_CMS.Models.Page
{
    public class Page
    {
        public Guid Id { get; set; }
        public string PageTitle { get; set; }
        public string PageBody { get; set; }
        public DateTime PageDate { get; set; }
        public string UserName { get; set; }
    }
}
