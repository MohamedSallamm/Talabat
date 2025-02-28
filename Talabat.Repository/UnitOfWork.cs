using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork<TEntity> : IUnitOfWork<TEntity>
    {
        private readonly StoreContext _dbContext;
        private Hashtable _Repository;

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _Repository = new Hashtable();
        }

        public IGenericRepository<TEntity> repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;

            if (!_Repository.Contains(key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _Repository.Add(key, repository);
            }
            return _Repository[key] as IGenericRepository<TEntity>;
        }

        public async Task<int> CompleteAsync()
               => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
                   => await _dbContext.DisposeAsync();

    }
}
