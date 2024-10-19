using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification(Expression<Func<T,bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDesc {  get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagnated { get; private set; }

        protected void AddIncludes(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderbyexpression)
        {
            OrderBy = orderbyexpression;
        }

        protected void AddOrderByDesc(Expression<Func<T, object>> orderbyDescexpression)
        {
            OrderByDesc = orderbyDescexpression;
        }

        protected void AddPaginated(int skip,int take)
        {
            Take = take;
            Skip = skip;
            IsPagnated = true;
        }
    }
}
