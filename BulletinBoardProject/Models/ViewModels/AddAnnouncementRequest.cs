namespace BulletinBoardProject.Models.ViewModels
{
    public class AddAnnouncementRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
