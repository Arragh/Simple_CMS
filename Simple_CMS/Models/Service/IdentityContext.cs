using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Simple_CMS.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.Models.Service
{
    public class IdentityContext : IdentityDbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        // Создаем начального пользователя Administrator с правами admin
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Создаем роль администратора
            IdentityRole adminRole = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                NormalizedName = "ADMIN"
            };

            // Создаем пользователя Администратора
            User adminUser = new User()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = IdentityConfig.AdminName,
                NormalizedUserName = IdentityConfig.NormalizedAdminName,
                Email = IdentityConfig.AdminEmail,
                NormalizedEmail = IdentityConfig.NormalizedAdminEmail,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, IdentityConfig.AdminPassword),
                EmailConfirmed = true,
                SecurityStamp = string.Empty
            };

            // Заносим роль в БД
            builder.Entity<IdentityRole>().HasData(adminRole);

            // Заносим пользователя в БД
            builder.Entity<User>().HasData(adminUser);

            // Пользователю Administrator присваиваем роль admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRole.Id,
                UserId = adminUser.Id
            });
        }
    }
}
