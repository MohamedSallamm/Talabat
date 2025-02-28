using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal static class SpecificationsEvaluator <TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery, ISpecefications<TEntity> Spec)
        {
            var Query = InputQuery;
            if(Spec.Criteria is not null)
                Query = Query.Where(Spec.Criteria);

            if (Spec.orderBy is not null)
               Query = Query.OrderBy(Spec.orderBy);

            else if (Spec.orderByDesc is not null)
               Query = Query.OrderByDescending(Spec.orderByDesc);

            if (Spec.isPaginationEnabled)
                Query = Query.Skip(Spec.Skip).Take(Spec.Take);

            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));

            return Query;

            
        }
    }
}
