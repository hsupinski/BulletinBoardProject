using BulletinBoardProject.Data;
using BulletinBoardProject.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoardProject.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly AnnouncementDbContext _context;

        public AnnouncementRepository(AnnouncementDbContext context)
        {
            _context = context;
        }

        public async Task<Announcement> AddAsync(Announcement announcement)
        {
            await _context.Announcements.AddAsync(announcement);
            await _context.SaveChangesAsync();
            return announcement;
        }

        public async Task<IEnumerable<Announcement>> GetFromTenDaysAgoAsync()
        {
            var tenDaysAgo = DateTime.Now.AddDays(-10);

            return await _context.Announcements
                .Where(a => a.CreatedAt >= tenDaysAgo)
                .ToListAsync();
        }

        public async Task<Announcement> GetByIdAsync(int id)
        {
            return await _context.Announcements
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
