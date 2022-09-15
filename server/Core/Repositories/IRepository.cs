namespace Konek.Server.Core.Repositories;

interface IRepository<T>
{
    public Task AddAsync(T item);

    public Task RemoveAsync(int id);

    public Task<T?> GetAsync(int id);

    public Task<List<T>> GetAllAsync();
}