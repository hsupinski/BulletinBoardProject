using BulletinBoardProject.Models.Domain;
using BulletinBoardProject.Models.ViewModels;
using BulletinBoardProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardProject.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementController(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAnnouncementRequest request)
        {
            var announcement = new Announcement
            {
                CreatedAt = DateTime.UtcNow,
                Title = request.Title,
                Description = request.Description,
            };

            await _announcementRepository.AddAsync(announcement);

            return RedirectToAction("Index", "Home");
        }
    }
}
