using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecefications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> orderBy { get; set; } = null;
        public Expression<Func<T, object>> orderByDesc { get; set; } = null;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool isPaginationEnabled { get; set; }


        public BaseSpecifications()
        {

        }
        public BaseSpecifications(Expression<Func<T, bool>> CriteriaExpression)
        {
            Criteria = CriteriaExpression; // P => P.id == 1
        }

        public void AddOrderByAsc(Expression<Func<T, object>> orderByExpression)
        {
            orderBy = orderByExpression;
        }


        public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
        {
            orderByDesc = orderByDescExpression;
        }

        public void ApplyPagination(int skip, int take)
        {
            isPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }

    }
}
