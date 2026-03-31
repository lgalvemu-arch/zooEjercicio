using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ZooMvc.Data.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly ZooDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public GenericRepository(ZooDbContext context)
    {
        Context = context;
        DbSet = Context.Set<TEntity>();
    }

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => DbSet.ToListAsync(cancellationToken);

    public async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        => await DbSet.FindAsync([id], cancellationToken);

    public Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => DbSet.Where(predicate).ToListAsync(cancellationToken);

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => DbSet.FirstOrDefaultAsync(predicate, cancellationToken);

    public IQueryable<TEntity> Query() => DbSet;
    public IQueryable<TEntity> QueryNoTracking() => DbSet.AsNoTracking();

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => DbSet.AddAsync(entity, cancellationToken).AsTask();

    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => DbSet.AddRangeAsync(entities, cancellationToken);

    public void Update(TEntity entity) => DbSet.Update(entity);
    public void Remove(TEntity entity) => DbSet.Remove(entity);
    public void RemoveRange(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => Context.SaveChangesAsync(cancellationToken);
}
