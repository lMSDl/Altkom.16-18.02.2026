namespace WebAPI.Services
{
    public interface ICrudService<T>
    {
        Task<int> CreateAsync(T entity);
        Task<T> ReadAsync(int id);
        Task<IEnumerable<T>> ReadAllAsync();
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(T entity);
    }
}
