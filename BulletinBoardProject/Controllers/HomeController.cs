using BulletinBoardProject.Models;
using BulletinBoardProject.Models.Domain;
using BulletinBoardProject.Models.ViewModels;
using BulletinBoardProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulletinBoardProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnnouncementRepository _announcementRepository;
        public HomeController(ILogger<HomeController> logger, IAnnouncementRepository announcementRepository)
        {
            _logger = logger;
            _announcementRepository = announcementRepository;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pageSize = 5; // set to 100 if needed
            var announcements = await _announcementRepository.GetFromTenDaysAgoAsync();

            var totalAnnouncements = announcements.Count();

            var announcementsDisplayed = announcements
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new AnnouncementListViewModel
            {
                Announcements = announcementsDisplayed.Select(a => new AnnouncementViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    CreatedAt = a.CreatedAt,
                    Description = a.Description
                }).ToList(),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalAnnouncements / (double)pageSize)
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
