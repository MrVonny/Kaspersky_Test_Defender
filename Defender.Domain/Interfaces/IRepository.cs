namespace Defender.Domain.Interfaces;

public interface IRepository<TEntity> : IDisposable where TEntity : class
{
    void Add(TEntity obj);
    Task<TEntity> GetById(int id);
    void Update(TEntity obj);
    void Remove(int id);
    int SaveChanges();
    Task<int> SaveChangesAsync();
}