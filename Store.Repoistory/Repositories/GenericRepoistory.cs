﻿using Microsoft.EntityFrameworkCore;
using Store.Data.Context;
using Store.Data.Entity;
using Store.Repoistory.Interfaces;
using Store.Repoistory.Specification;
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

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
            => await _context.Set<TEntity>().ToListAsync();

        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecificationAsync(ISpecification<TEntity> specification)
            => await SpecificationEvaluator<TEntity,TKey>.GetQuery(_context.Set<TEntity>(),specification)
            .ToListAsync();

        public async Task<TEntity> GetByIdAsync(TKey? Id)
            => await _context.Set<TEntity>().FindAsync(Id);

        public async Task<TEntity> GetByIdWithSpecificationAsync(ISpecification<TEntity> specification)
            => await SpecificationEvaluator<TEntity,TKey>.GetQuery(_context.Set<TEntity>(),specification)
            .FirstOrDefaultAsync();

        public async Task<int> GetCountSpecificationAsync(ISpecification<TEntity> specification)
            => await SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), specification)
            .CountAsync();

        public void Update(TEntity entity)
            => _context.Set<TEntity>().Update(entity);
    }
}
