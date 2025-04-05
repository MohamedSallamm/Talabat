//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Query;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Talabat.Core.Entities;
//using Talabat.Core.Specifications;

//namespace Talabat.Repository
//{
//    internal static class SpecificationsEvaluator <TEntity> where TEntity : BaseEntity
//    {
//        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery, ISpecefications<TEntity> Spec)
//        {
//            var Query = InputQuery;
//            if(Spec.Criteria is not null)
//                Query = Query.Where(Spec.Criteria);

//            if (Spec.orderBy is not null)
//               Query = Query.OrderBy(Spec.orderBy);

//            else if (Spec.orderByDesc is not null)
//               Query = Query.OrderByDescending(Spec.orderByDesc);

//            if (Spec.isPaginationEnabled)
//                Query = Query.Skip(Spec.Skip).Take(Spec.Take);

//            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));

//            return Query;


//        }
//    }
//}



using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecefications<TEntity> spec)
        {
            var query = inputQuery;

            // تطبيق الفلترة
            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            // تطبيق الترتيب
            if (spec.orderBy is not null)
                query = query.OrderBy(spec.orderBy);
            else if (spec.orderByDesc is not null)
                query = query.OrderByDescending(spec.orderByDesc);

            // **تطبيق الترحيل (Pagination) قبل `Include()`**
            if (spec.isPaginationEnabled)
            {
                Console.WriteLine($"Applying Pagination: Skip = {spec.Skip}, Take = {spec.Take}");
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // **تطبيق `Include()` بعد `Skip` و `Take`**
            query = spec.Includes.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));

            return query;
        }
    }
}
