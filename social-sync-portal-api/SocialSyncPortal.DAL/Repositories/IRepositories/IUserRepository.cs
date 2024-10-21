using SocialSyncPortal.DAL.Entities;

namespace SocialSyncPortal.DAL.Repositories.IRepositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> UpdateUserAsync(User user);
}