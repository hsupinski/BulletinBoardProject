using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardProject.Models.ViewModels
{
    public class AnnouncementViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
