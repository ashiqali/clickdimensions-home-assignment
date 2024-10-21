using SocialSyncPortal.DAL.DataContext;
using SocialSyncPortal.DAL.Entities;
using SocialSyncPortal.DAL.Repositories.IRepositories;

namespace SocialSyncPortal.DAL.Repositories
{
    public class SocialPostRepository : GenericRepository<SocialPost>, ISocialPostRepository
    {
        public SocialPostRepository(SocialSyncPortalDbContext SocialSyncPortalDbContext) : base(SocialSyncPortalDbContext)
        {
        }
    }
}
