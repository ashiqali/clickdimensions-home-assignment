using SocialSyncPortal.DAL.DataContext;
using SocialSyncPortal.DAL.Entities;
using SocialSyncPortal.DAL.Repositories.IRepositories;

namespace SocialSyncPortal.DAL.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly SocialSyncPortalDbContext _SocialSyncPortalDbContext;
    public UserRepository(SocialSyncPortalDbContext SocialSyncPortalDbContext) : base(SocialSyncPortalDbContext)
    {
        _SocialSyncPortalDbContext = SocialSyncPortalDbContext;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        _ = _SocialSyncPortalDbContext.Update(user);

        // Ignore password property update for user
        _SocialSyncPortalDbContext.Entry(user).Property(x => x.Password).IsModified = false;

        await _SocialSyncPortalDbContext.SaveChangesAsync();
        return user;
    }
}
