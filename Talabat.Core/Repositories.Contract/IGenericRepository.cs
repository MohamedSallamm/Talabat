using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        /// <summary>
        /// Get All (Products, Categoyries, Brands)
        /// </summary>
        /// <param name="spec">Entity model</param>
        /// <returns> Mohamed sallam</returns>
        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecefications<T> spec);
        Task<T?> GetEntityWithSpecAsync(ISpecefications<T> spec);


        Task<int> GetCountAsync(ISpecefications<T> spec);


        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
