using SocialSyncPortal.DAL.DataContext;
using SocialSyncPortal.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SocialSyncPortal.DAL.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
{
    private readonly SocialSyncPortalDbContext _SocialSyncPortalDbContext;
    public GenericRepository(SocialSyncPortalDbContext SocialSyncPortalDbContext)
    {
        _SocialSyncPortalDbContext = SocialSyncPortalDbContext;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _SocialSyncPortalDbContext.AddAsync(entity);
        await _SocialSyncPortalDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entity)
    {
        await _SocialSyncPortalDbContext.AddRangeAsync(entity);
        await _SocialSyncPortalDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<int> DeleteAsync(TEntity entity)
    {
        _ = _SocialSyncPortalDbContext.Remove(entity);
        return await _SocialSyncPortalDbContext.SaveChangesAsync();
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
    {
        return await _SocialSyncPortalDbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
    {
        return await (filter == null ? _SocialSyncPortalDbContext.Set<TEntity>().ToListAsync(cancellationToken) : _SocialSyncPortalDbContext.Set<TEntity>().Where(filter).ToListAsync(cancellationToken));
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _ = _SocialSyncPortalDbContext.Update(entity);
        await _SocialSyncPortalDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entity)
    {
        _SocialSyncPortalDbContext.UpdateRange(entity);
        await _SocialSyncPortalDbContext.SaveChangesAsync();
        return entity;
    }
}
