using Store.Data.Context;
using Store.Data.Entity;
using Store.Repoistory.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        private Hashtable _repository;

        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public IGenericRepoistory<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
           
            if(_repository is null)
            {
                _repository = new Hashtable();
            }

            var entityKey = typeof(TEntity).Name;

            if (!_repository.ContainsKey(entityKey))
            {
                var repositoryType = typeof(GenericRepoistory<,>); //GenericRepoistory<product,int>

                //Class Activator => Make me instance of GenericRepoistory thar determine in repositoryType
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity),typeof(TKey)),_context);

                _repository.Add(entityKey, repositoryInstance);
            }

            return (IGenericRepoistory<TEntity, TKey>)_repository[entityKey];
        }
    }
}
