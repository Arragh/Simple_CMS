using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simple_CMS.Models.Identity;
using Simple_CMS.Models.Service;

namespace Simple_CMS
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.Bind("Simple_CMS_WebsiteSettings", new WebsiteConfig());
            Configuration.Bind("Simple_CMS_IdentitySettings", new IdentityConfig());

            services.AddDbContext<WebsiteContext>(options => options.UseSqlServer(WebsiteConfig.SimpleCMS_WebsiteDB));

            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(WebsiteConfig.SimpleCMS_IdentityDB));
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = IdentityConfig.AllowedUserNameCharacters;
            })
                .AddEntityFrameworkStores<IdentityContext>();

            services.AddControllersWithViews().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=News}/{action=Index}/{id?}");
            });
        }
    }
}
