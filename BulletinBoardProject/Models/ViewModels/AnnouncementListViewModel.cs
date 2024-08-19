namespace BulletinBoardProject.Models.ViewModels
{
    public class AnnouncementListViewModel
    {
        public List<AnnouncementViewModel> Announcements { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
