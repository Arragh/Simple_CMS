using Microsoft.EntityFrameworkCore;
using Simple_CMS.Models.Admin;
using Simple_CMS.Models.Gallery;
using Simple_CMS.Models.News;
using System;

namespace Simple_CMS.Models.Service
{
    public class WebsiteContext : DbContext
    {
        public DbSet<News.News> News { get; set; }
        public DbSet<NewsImage> NewsImages { get; set; }
        public DbSet<Gallery.Gallery> Galleries { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<Page.Page> Pages { get; set; }
        public DbSet<Settings> Settings { get; set; }

        public WebsiteContext(DbContextOptions<WebsiteContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            Settings baseSettings = new Settings()
            {
                Id = Guid.NewGuid(),
                NewsImageSize = 2097152,
                ImagesPerNews = 3,
                NewsPerPage = 10,
                SettingsName = "baseSettings"
            };

            builder.Entity<Settings>().HasData(baseSettings);
        }
    }
}
