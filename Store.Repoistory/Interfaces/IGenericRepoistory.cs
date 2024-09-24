using Store.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory.Interfaces
{
    public interface IGenericRepoistory<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey? Id);

        Task<IReadOnlyList<TEntity>> GetAllAsync();

        Task<IReadOnlyList<TEntity>> GetAllAsNoTrackingAsync();

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
