using BulletinBoardProject.Models.Domain;

namespace BulletinBoardProject.Repositories
{
    public interface IAnnouncementRepository
    {
        Task<IEnumerable<Announcement>> GetFromTenDaysAgoAsync();
        Task<Announcement> AddAsync(Announcement announcement);
    }
}
