﻿using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public interface ISpecefications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public Expression<Func<T, object>> orderBy { get; set; }
        public Expression<Func<T, object>> orderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool isPaginationEnabled { get; set; }
    }
}
