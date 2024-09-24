using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Store.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepoistory<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;

        Task<int> CompleteAsync();
    }
}
