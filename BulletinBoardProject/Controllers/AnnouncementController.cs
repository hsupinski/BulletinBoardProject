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
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var announcement = await _announcementRepository.GetByIdAsync(id);
            return View(announcement);
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

            if (string.IsNullOrEmpty(announcement.Title) || string.IsNullOrEmpty(announcement.Description))
            {
                TempData["ErrorMessage"] = "Failed to add announcement. Title and description are required.";
                return RedirectToAction("Create");
            }

            await _announcementRepository.AddAsync(announcement);

            TempData["SuccessMessage"] = "Announcement added successfully!";
            return RedirectToAction("Create");
        }
    }
}
