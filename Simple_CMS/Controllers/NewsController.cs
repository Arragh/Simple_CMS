﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_CMS.Models.Admin;
using Simple_CMS.Models.News;
using Simple_CMS.Models.Service;
using Simple_CMS.ViewModels.News;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_CMS.Controllers
{
    [Authorize(Roles = "admin")]
    public class NewsController : Controller
    {
        private WebsiteContext _websiteDB;
        private IWebHostEnvironment _appEnvironment;

        public NewsController(WebsiteContext websiteDBContext, IWebHostEnvironment appEnvironment)
        {
            _websiteDB = websiteDBContext;
            _appEnvironment = appEnvironment;
        }

        #region Главная страница
        [AllowAnonymous]
        public async Task<IActionResult> Index(int pageNumber = 1) // Страница по умолчанию = 1
        {
            Settings settings = await _websiteDB.Settings.FirstAsync(s => s.SettingsName == "baseSettings");

            // Количество записей на страницу
            int pageSize = settings.NewsPerPage;

            // Формируем список записей для обработки перед выводом на страницу
            IQueryable<News> source = _websiteDB.News;

            // Рассчитываем, какие именно записи будут выведены на странице
            List<News> news = await source.OrderByDescending(n => n.NewsDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Общее количество записей для дальнейшего рассчета количества страниц
            int newsCount = await source.CountAsync();

            // Создаем массив Id-шников записей для выборки изображений к ним
            Guid[] newsIdArray = news.Select(n => n.Id).ToArray();

            // Создаем список из изображений, которые будут поданы вместе со списом записей
            List<NewsImage> newsImages = new List<NewsImage>();

            // Перебираем изображения в БД
            foreach (var image in _websiteDB.NewsImages)
            {
                // Перебираем все элементы ранее созданного массива Guid[] newsIdArray
                foreach (var newsId in newsIdArray)
                {
                    // Если данные совпадают, то кладём изображение в список для вывода на странице
                    if (image.NewsId == newsId)
                    {
                        newsImages.Add(image);
                    }
                }
            }

            // Создаём модель для вывода на странице и кладём в неё все необходимые данные
            IndexViewModel model = new IndexViewModel()
            {
                News = news,
                NewsImages = newsImages,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(newsCount / (double)pageSize)
            };

            // Выводим модель в представление
            return View(model);
        }
        #endregion

        #region Создать новость [GET]
        [HttpGet]
        public async Task<IActionResult> AddNews()
        {
            Settings settings = await _websiteDB.Settings.FirstAsync(s => s.SettingsName == "baseSettings");
            ViewBag.ImagesPerNews = settings.ImagesPerNews;

            ViewBag.Title = "Создать новость";

            return View();
        }
        #endregion

        #region Создать новость [POST]
        [HttpPost]
        public async Task<IActionResult> AddNews(AddNewsViewModel model, IFormFileCollection uploads)
        {
            Settings settings = await _websiteDB.Settings.FirstAsync(s => s.SettingsName == "baseSettings");
            ViewBag.ImagesPerNews = settings.ImagesPerNews;

            // Проверяем, чтобы размер файлов не превышал заданный объем
            foreach (var file in uploads)
            {
                if (file.Length > settings.NewsImageSize) // NewsImageLimit !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                {
                    ModelState.AddModelError("NewsImage", $"Файл \"{file.FileName}\" превышает установленный лимит 2MB.");
                    break;
                }
            }

            // Если все в порядке, заходим в тело условия
            if (ModelState.IsValid)
            {
                // Создаем экземпляр класса News и присваиваем ему значения
                News news = new News()
                {
                    Id = Guid.NewGuid(),
                    NewsTitle = model.NewsTitle,
                    NewsBody = model.NewsBody,
                    NewsDate = DateTime.Now,
                    UserName = User.Identity.Name
                };

                // Далее начинаем обработку изображений
                List<NewsImage> newsImages = new List<NewsImage>();
                foreach (var uploadedImage in uploads)
                {
                    // Если размер входного файла больше 0, заходим в тело условия
                    if (uploadedImage.Length > 0)
                    {
                        // Создаем новый объект класса FileInfo из полученного изображения для дальнейшей обработки
                        FileInfo imgFile = new FileInfo(uploadedImage.FileName);
                        // Приводим расширение к нижнему регистру (если оно было в верхнем)
                        string imgExtension = imgFile.Extension.ToLower();
                        // Генерируем новое имя для файла
                        string newFileName = Guid.NewGuid() + imgExtension;
                        // Пути сохранения файла
                        string pathNormal = "/files/images/normal/" + newFileName; // изображение исходного размера
                        string pathScaled = "/files/images/scaled/" + newFileName; // уменьшенное изображение

                        // В операторе try/catch делаем уменьшенную копию изображения.
                        // Если входным файлом окажется не изображение, нас перекинет в блок CATCH и выведет сообщение об ошибке
                        try
                        {
                            // Создаем объект класса SixLabors.ImageSharp.Image и грузим в него полученное изображение
                            using (Image image = Image.Load(uploadedImage.OpenReadStream()))
                            {
                                // Создаем уменьшенную копию и обрезаем её
                                var clone = image.Clone(x => x.Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Crop,
                                    Size = new Size(300, 200)
                                }));
                                // Сохраняем уменьшенную копию
                                await clone.SaveAsync(_appEnvironment.WebRootPath + pathScaled, new JpegEncoder { Quality = 50 });
                                // Сохраняем исходное изображение
                                await image.SaveAsync(_appEnvironment.WebRootPath + pathNormal);
                            }

                        }
                        // Если вдруг что-то пошло не так (например, на вход подало не картинку), то выводим сообщение об ошибке
                        catch
                        {
                            // Создаем сообщение об ошибке для вывода пользователю
                            ModelState.AddModelError("NewsImage", $"Файл {uploadedImage.FileName} имеет неверный формат.");

                            // Удаляем только что созданные файлы (если ошибка возникла не на первом файле)
                            foreach (var image in newsImages)
                            {
                                // Исходные (полноразмерные) изображения
                                FileInfo imageNormal = new FileInfo(_appEnvironment.WebRootPath + image.ImagePathNormal);
                                if (imageNormal.Exists)
                                {
                                    imageNormal.Delete();
                                }
                                // И их уменьшенные копии
                                FileInfo imageScaled = new FileInfo(_appEnvironment.WebRootPath + image.ImagePathScaled);
                                if (imageScaled.Exists)
                                {
                                    imageScaled.Delete();
                                }
                            }

                            // Возвращаем модель с сообщением об ошибке в представление
                            return View(model);
                        }

                        // Создаем объект класса NewsImage со всеми параметрами
                        NewsImage newsImage = new NewsImage()
                        {
                            Id = Guid.NewGuid(),
                            ImageName = newFileName,
                            ImagePathNormal = pathNormal,
                            ImagePathScaled = pathScaled,
                            NewsId = news.Id
                        };
                        // Добавляем объект newsImage в список newsImages
                        newsImages.Add(newsImage);
                    }
                }

                // Если в процессе выполнения не возникло ошибок, сохраняем всё в БД
                if (newsImages != null && newsImages.Count > 0)
                {
                    await _websiteDB.NewsImages.AddRangeAsync(newsImages);
                }
                await _websiteDB.News.AddAsync(news);
                await _websiteDB.SaveChangesAsync();

                // Редирект на главную страницу
                return RedirectToAction("Index", "News");
            }

            // Возврат модели в представление в случае, если запорится валидация
            return View(model);
        }
        #endregion

        #region Редактировать новость [GET]
        [HttpGet]
        public async Task<IActionResult> EditNews(Guid newsId, string imageToDeleteName = null)
        {
            // Если есть изображение, которое надо удалить, заходим в тело условия
            if (imageToDeleteName != null)
            {
                // Создаем экземпляр класса картинки и присваиваем ему данные из БД
                NewsImage newsImage = await _websiteDB.NewsImages.FirstOrDefaultAsync(i => i.ImageName == imageToDeleteName);

                // Делаем еще одну проверку. Лучше перебдеть. Если все ок, заходим в тело условия и удаляем изображения
                if (newsImage != null)
                {
                    // Исходные (полноразмерные) изображения
                    FileInfo imageNormal = new FileInfo(_appEnvironment.WebRootPath + newsImage.ImagePathNormal);
                    if (imageNormal.Exists)
                    {
                        imageNormal.Delete();
                    }
                    // И их уменьшенные копии
                    FileInfo imageScaled = new FileInfo(_appEnvironment.WebRootPath + newsImage.ImagePathScaled);
                    if (imageScaled.Exists)
                    {
                        imageScaled.Delete();
                    }
                    // Удаляем информацию об изображениях из БД и сохраняем
                    _websiteDB.NewsImages.Remove(newsImage);
                    await _websiteDB.SaveChangesAsync();
                }
            }

            // Создаем экземпляр класса News и присваиваем ему значения из БД
            News news = await _websiteDB.News.FirstAsync(n => n.Id == newsId);
            // Создаем список изображений из БД, закрепленных за выбранной новостью
            List<NewsImage> images = await _websiteDB.NewsImages.Where(i => i.NewsId == newsId).OrderByDescending(i => i.ImageDate).ToListAsync();

            // Создаем модель для передачи в представление и присваиваем значения
            EditNewsViewModel model = new EditNewsViewModel()
            {
                NewsTitle = news.NewsTitle,
                NewsBody = news.NewsBody,
                NewsImages = images,
                // Скрытые поля
                NewsId = newsId,
                NewsDate = news.NewsDate,
                UserName = news.UserName,
                ImagesCount = images.Count
            };

            ViewBag.Title = $"Редактирование новости \"{news.NewsTitle}\"";

            // Передаем модель в представление
            return View(model);
        }
        #endregion

        #region Редактировать новость [POST]
        [HttpPost]
        public async Task<IActionResult> EditNews(EditNewsViewModel model, IFormFileCollection uploads)
        {
            // Проверяем, чтобы размер файлов не превышал заданный объем
            foreach (var file in uploads)
            {
                if (file.Length > 2097152)
                {
                    ModelState.AddModelError("NewsImage", $"Файл \"{file.FileName}\" превышает установленный лимит 2MB.");
                    break;
                }
            }

            // Если все в порядке, заходим в тело условия
            if (ModelState.IsValid)
            {
                // Создаем экземпляр класса News и присваиваем ему значения
                News news = new News()
                {
                    NewsTitle = model.NewsTitle,
                    NewsBody = model.NewsBody,
                    // Скрытые поля
                    Id = model.NewsId,
                    NewsDate = model.NewsDate,
                    UserName = model.UserName
                };

                // Далее начинаем обработку загружаемых изображений
                List<NewsImage> newsImages = new List<NewsImage>();
                foreach (var uploadedImage in uploads)
                {
                    // Если размер входного файла больше 0, заходим в тело условия
                    if (uploadedImage.Length > 0)
                    {
                        // Создаем новый объект класса FileInfo из полученного изображения для дальнейшей обработки
                        FileInfo imgFile = new FileInfo(uploadedImage.FileName);
                        // Приводим расширение к нижнему регистру (если оно было в верхнем)
                        string imgExtension = imgFile.Extension.ToLower();
                        // Генерируем новое имя для файла
                        string newFileName = Guid.NewGuid() + imgExtension;
                        // Пути сохранения файла
                        string pathNormal = "/files/images/normal/" + newFileName; // изображение исходного размера
                        string pathScaled = "/files/images/scaled/" + newFileName; // уменьшенное изображение

                        // В операторе try/catch делаем уменьшенную копию изображения.
                        // Если входным файлом окажется не изображение, нас перекинет в блок CATCH и выведет сообщение об ошибке
                        try
                        {
                            // Создаем объект класса SixLabors.ImageSharp.Image и грузим в него полученное изображение
                            using (Image image = Image.Load(uploadedImage.OpenReadStream()))
                            {
                                // Создаем уменьшенную копию и обрезаем её
                                var clone = image.Clone(x => x.Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Crop,
                                    Size = new Size(300, 200)
                                }));
                                // Сохраняем уменьшенную копию
                                await clone.SaveAsync(_appEnvironment.WebRootPath + pathScaled, new JpegEncoder { Quality = 50 });
                                // Сохраняем исходное изображение
                                await image.SaveAsync(_appEnvironment.WebRootPath + pathNormal);
                            }
                        }
                        // Если вдруг что-то пошло не так (например, на вход подало не картинку), то выводим сообщение об ошибке
                        catch
                        {
                            // Создаем сообщение об ошибке для вывода пользователю
                            ModelState.AddModelError("NewsImage", $"Файл {uploadedImage.FileName} имеет неверный формат.");

                            // Удаляем только что созданные файлы (если ошибка возникла не на первом файле)
                            foreach (var image in newsImages)
                            {
                                // Исходные (полноразмерные) изображения
                                FileInfo imageNormal = new FileInfo(_appEnvironment.WebRootPath + image.ImagePathNormal);
                                if (imageNormal.Exists)
                                {
                                    imageNormal.Delete();
                                }
                                // И их уменьшенные копии
                                FileInfo imageScaled = new FileInfo(_appEnvironment.WebRootPath + image.ImagePathScaled);
                                if (imageScaled.Exists)
                                {
                                    imageScaled.Delete();
                                }
                            }
                            // Возвращаем модель с сообщением об ошибке в представление
                            return View(model);
                        }

                        // Создаем объект класса NewsImage со всеми параметрами
                        NewsImage newsImage = new NewsImage()
                        {
                            Id = Guid.NewGuid(),
                            ImageName = newFileName,
                            ImagePathNormal = pathNormal,
                            ImagePathScaled = pathScaled,
                            NewsId = news.Id,
                            ImageDate = DateTime.Now
                        };
                        // Добавляем объект newsImage в список newsImages
                        newsImages.Add(newsImage);
                    }
                }

                // Если в процессе выполнения не возникло ошибок, сохраняем всё в БД
                if (newsImages != null && newsImages.Count > 0)
                {
                    await _websiteDB.NewsImages.AddRangeAsync(newsImages);
                }
                _websiteDB.News.Update(news);
                await _websiteDB.SaveChangesAsync();

                // Редирект на главную страницу
                return RedirectToAction("Index", "News");
            }

            // В случае, если при редактировании пытаться загрузить картинку выше разрешенного лимита, то перестают отображаться уже имеющиеся изображения
            // При перегонке модели из гет в пост, теряется список с изображениями. Причина пока не ясна, поэтому сделал такой костыль
            // Счетчик соответственно тоже обнулялся, поэтому его тоже приходится переназначать заново
            List<NewsImage> images = await _websiteDB.NewsImages.Where(i => i.NewsId == model.NewsId).OrderByDescending(i => i.ImageDate).ToListAsync();
            model.NewsImages = images;
            model.ImagesCount = images.Count;

            // Возврат модели в представление в случае, если запорится валидация
            return View(model);
        }
        #endregion

        #region Удалить новость [POST]
        public async Task<IActionResult> DeleteNews(Guid newsId, bool isChecked, int imagesCount = 0)
        {
            // Если удаление подтверждено, заходим в условия
            if (isChecked)
            {
                // Создаем экземпляр новости для удаления. Достаточно просто присвоить Id удаляемой записи
                News news = new News { Id = newsId };

                // Проверяем, присутствуют ли в новости изображения
                if (imagesCount > 0)
                {
                    // Создаем список из привязанных к удаляемой записи изображений
                    List<NewsImage> newsImages = await _websiteDB.NewsImages.Where(i => i.NewsId == newsId).ToListAsync();

                    // Удаление изображений из папок
                    foreach (var image in newsImages)
                    {
                        // Исходные (полноразмерные) изображения
                        FileInfo imageNormal = new FileInfo(_appEnvironment.WebRootPath + image.ImagePathNormal);
                        if (imageNormal.Exists)
                        {
                            imageNormal.Delete();
                        }
                        // И их уменьшенные копии
                        FileInfo imageScaled = new FileInfo(_appEnvironment.WebRootPath + image.ImagePathScaled);
                        if (imageScaled.Exists)
                        {
                            imageScaled.Delete();
                        }
                    }

                    // Удаляем изображения из БД
                    // Эта строка не обязательна, т.к. при удалении новости, записи с изображениями теряют связь по Id и трутся сами
                    // Достаточно просто удалить изображения из папок, что уже сделано выше
                    _websiteDB.NewsImages.RemoveRange(newsImages);
                }

                // Удаляем новость из БД
                _websiteDB.News.Remove(news);
                await _websiteDB.SaveChangesAsync();
            }

            return RedirectToAction("Index", "News");
        }
        #endregion
    }
}
