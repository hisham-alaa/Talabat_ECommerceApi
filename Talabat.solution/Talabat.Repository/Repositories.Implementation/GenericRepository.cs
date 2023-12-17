using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Reporitories.Contract;
using Talabat.Core.Sepecifications;
using Talabat.Repository.Data.Contexts;

namespace Talabat.Repository.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase
    {
        private readonly StoreContext context;

        public GenericRepository(StoreContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T) == typeof(Product))//Must be solved using the specification Design Pattern
            //    return (IEnumerable<T>)await context.Set<Product>().Include(p => p.Brand).Include(p => p.Category).ToListAsync();

            return await context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecification<T> spec)
        {
            return await ApplySpec(spec).ToListAsync();
        }

        public async Task<T?> GetAsyncWithSpec(ISpecification<T> spec)
        {
            return await ApplySpec(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsyncWithSpec(ISpecification<T> spec)
        {
            return await ApplySpec(spec).CountAsync();
        }

        public IQueryable<T> ApplySpec(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.BuildQuery(context.Set<T>(), spec);
        }

        public async Task AddAsync(T item)
            => await context.Set<T>().AddAsync(item);

        public void Update(T item)
            => context.Set<T>().Update(item);

        public void Delete(T item)
            => context.Set<T>().Remove(item);
    }
}
