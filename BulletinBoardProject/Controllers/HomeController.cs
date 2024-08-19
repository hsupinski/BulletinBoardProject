using BulletinBoardProject.Models;
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

        public IActionResult Index()
        {
            var announcementList = _announcementRepository.GetFromTenDaysAgoAsync();

            return View();
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
