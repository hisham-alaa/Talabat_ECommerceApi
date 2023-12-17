using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Sepecifications;

namespace Talabat.Core.Reporitories.Contract
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecification<T> spec);
        Task<T?> GetAsyncWithSpec(ISpecification<T> spec);

        Task<int> GetCountAsyncWithSpec(ISpecification<T> spec);

        Task AddAsync(T item);

        void Update(T item);

        void Delete(T item);

    }
}
