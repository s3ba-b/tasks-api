using System.Linq.Expressions;

namespace TasksApi.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        void Update(T entity);
        void Remove(T entity);
        Task SaveChangesAsync();
        Task<bool> ExistsAsync(int id);
    }
}