using SocialSyncPortal.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace SocialSyncPortal.DAL.DataContext
{
    public class SocialSyncPortalDbContext : DbContext
    {
        public SocialSyncPortalDbContext(DbContextOptions<SocialSyncPortalDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<SocialPost> SocialPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Sample Data for User
            modelBuilder.Entity<User>().HasData(
                 new User
                 {
                     Id = 1,
                     Username = "admin",
                     Password = "123",
                     Name = "Admin",
                     Surname = "Admin",
                 }
             );
            #endregion Sample Data for User           
        }
    }
}
