using Microsoft.EntityFrameworkCore;
using Simple_CMS.Models.Gallery;
using Simple_CMS.Models.News;

namespace Simple_CMS.Models.Service
{
    public class WebsiteContext : DbContext
    {
        public DbSet<News.News> News { get; set; }
        public DbSet<NewsImage> NewsImages { get; set; }
        public DbSet<Gallery.Gallery> Galleries { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<Page.Page> Pages { get; set; }

        public WebsiteContext(DbContextOptions<WebsiteContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
