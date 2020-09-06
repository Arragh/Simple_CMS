using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple_CMS.Models.Admin;
using Simple_CMS.Models.Service;
using Simple_CMS.ViewModels.Admin;

namespace Simple_CMS.Controllers
{
    public class AdminController : Controller
    {
        private WebsiteContext _websiteDB;

        public AdminController(WebsiteContext websiteDBContext)
        {
            _websiteDB = websiteDBContext;
        }

        [HttpGet]
        public IActionResult Settings()
        {
            Settings settings = _websiteDB.Settings.First(s => s.SettingsName == "baseSettings");
            SettingsViewModel model = new SettingsViewModel()
            {
                NewsImageSize = settings.NewsImageSize,
                ImagesPerNews = settings.ImagesPerNews,
                NewsPerPage = settings.NewsPerPage
            };

            ViewBag.Title = "Настройки";

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(SettingsViewModel model)
        {
            Settings settings = _websiteDB.Settings.First(s => s.SettingsName == "baseSettings");

            settings.NewsImageSize = model.NewsImageSize;
            settings.ImagesPerNews = model.ImagesPerNews;
            settings.NewsPerPage = model.NewsPerPage;

            _websiteDB.Update(settings);
            await _websiteDB.SaveChangesAsync();

            return RedirectToAction("Settings", "Admin");
        }
    }
}
