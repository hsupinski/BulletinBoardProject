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

            await _announcementRepository.AddAsync(announcement);

            // Store success message in localStorage
            TempData["SuccessMessage"] = "Announcement added successfully!";
            HttpContext.Response.Cookies.Append("SuccessMessage", "Announcement added successfully!");

            return RedirectToAction("Create");
        }
    }
}
