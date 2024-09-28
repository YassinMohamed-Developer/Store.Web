using Microsoft.EntityFrameworkCore;
using Store.Data.Context;
using Store.Data.Entity;
using Store.Repoistory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory.Repositories
{
    public class GenericRepoistory<TEntity, TKey> : IGenericRepoistory<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _context;

        public GenericRepoistory(StoreDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TEntity entity)
            =>  await _context.AddAsync(entity);

        public void Delete(TEntity entity)
            => _context.Set<TEntity>().Remove(entity);

        public async Task<IReadOnlyList<TEntity>> GetAllAsNoTrackingAsync()
            => await _context.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(string? sort,string? search)
        {
            if(typeof(TEntity) == typeof(Product))
            {

                if (!string.IsNullOrEmpty(search))
                {
                     return (IReadOnlyList<TEntity>) await _context.Products.Where(x => x.Name.Contains(search.Trim().ToLower()))
                        .Include(X => X.Brand)
                        .Include(X => X.Type).ToListAsync();
                } 
                
                if (sort == "PriceAsc")
                {
                    return (IReadOnlyList<TEntity>) await _context.Products.OrderBy(x => x.Price)
                        .Include(X => X.Brand)
                        .Include(X => X.Type).ToListAsync();
                }
                else if (sort == "PriceDesc")
                {
                    return (IReadOnlyList<TEntity>)await _context.Products.OrderByDescending(x => x.Price)
                        .Include(X => X.Brand)
                        .Include(X => X.Type).ToListAsync();
                }
                return (IReadOnlyList<TEntity>)await _context.Products.Include(X => X.Brand).Include(X => X.Type).ToListAsync();
            }

             return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(TKey? Id)
            => await _context.Set<TEntity>().FindAsync(Id);

        //public async Task<IReadOnlyList<TEntity>> GetProductByOrderAsc(string? sort)
        //{
        //    if (typeof(TEntity) == typeof(Product))
        //    {
        //        if(sort == "PriceAsc")
        //        {
        //           return (IReadOnlyList<TEntity>)await _context.Products.OrderBy(x => x.Price).ToListAsync();
        //        }
        //       else if(sort =="PriceDesc")
        //       {
        //            return (IReadOnlyList<TEntity>)await _context.Products.OrderByDescending(x => x.Price).ToListAsync();
        //       }
        //    }
        //    return await _context.Set<TEntity>().OrderBy(X => X.Id).ToListAsync();
        //}

        public void Update(TEntity entity)
            => _context.Set<TEntity>().Update(entity);
    }
}
