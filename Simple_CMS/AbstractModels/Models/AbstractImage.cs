using System;

namespace Simple_CMS.AbstractModels.Models
{
    public abstract class AbstractImage
    {
        public Guid Id { get; set; }
        public string ImageName { get; set; }
        public string ImagePathNormal { get; set; }
        public string ImagePathScaled { get; set; }
        public DateTime ImageDate { get; set; }
    }
}
