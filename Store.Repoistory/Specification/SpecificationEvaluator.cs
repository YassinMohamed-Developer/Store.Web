using Microsoft.EntityFrameworkCore;
using Store.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory.Specification
{
    public class SpecificationEvaluator<TEntity,TKey>  where TEntity : BaseEntity<TKey>
    {
        //_context.products = IQueryable<T> inputquery

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputquery,ISpecification<TEntity> specs)
        {

            var query = inputquery;

          if(specs.Criteria is not null)
                query = query.Where(specs.Criteria); // x=>x.typeid == 3

            if (specs.OrderBy is not null)
                query = query.OrderBy(specs.OrderBy);

            if (specs.OrderByDesc is not null)
                query = query.OrderByDescending(specs.OrderByDesc);

            if(specs.IsPagnated)
                query = query.Skip(specs.Skip).Take(specs.Take);

            query = specs.Includes.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));

            return query;
        }
    }
}
