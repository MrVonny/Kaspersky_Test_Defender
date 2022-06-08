using Defender.Domain.Interfaces;
using Defender.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Defender.Infrastructure.Data.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(ApplicationDbContext context)
    {
        Db = context;
        DbSet = Db.Set<TEntity>();
    }

    public virtual void Add(TEntity obj)
    {
        DbSet.Add(obj);
    }

    public virtual async Task<TEntity> GetById(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual void Update(TEntity obj)
    {
        DbSet.Update(obj);
    }

    public virtual void Remove(int id)
    {
        DbSet.Remove(DbSet.Find(id));
    }

    public int SaveChanges()
    {
        return Db.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Db.SaveChangesAsync();
    }

    public void Dispose()
    {
        Db.Dispose();
        GC.SuppressFinalize(this);
    }
}