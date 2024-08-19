using BulletinBoardProject.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoardProject.Data
{
    public class AnnouncementDbContext : DbContext
    {
        public AnnouncementDbContext(DbContextOptions<AnnouncementDbContext> options) : base(options) { }
        public DbSet<Announcement> Announcements { get; set; }
    }
}
