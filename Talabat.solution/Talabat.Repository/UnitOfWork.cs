using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Reporitories.Contract;
using Talabat.Repository.Data.Contexts;
using Talabat.Repository.Repositories.Implementation;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _storeContext;

        private Hashtable _repos;

        public UnitOfWork(StoreContext storeContext)
        {
            _storeContext = storeContext;
            _repos = new Hashtable();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : EntityBase
        {
            var keyName = typeof(TEntity).Name;
            if (!_repos.ContainsKey(keyName))
            {
                var repository = new GenericRepository<TEntity>(_storeContext);
                _repos.Add(keyName, repository);
            }
            return _repos[keyName] as IGenericRepository<TEntity>;
        }

        public async Task<int> CompleteAsync()
            => await _storeContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _storeContext.DisposeAsync();

    }
}
