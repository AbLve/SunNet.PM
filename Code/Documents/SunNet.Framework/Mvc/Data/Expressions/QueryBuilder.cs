using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SF.Framework.Mvc.Data.Expressions
{
    public static class QueryBuilder
    {
        public static IQueryBuilder<T> Create<T>()
        {
            return new QueryBuilder<T>();
        }
    }

    class QueryBuilder<T> : IQueryBuilder<T>
    {
        private Expression<Func<T, bool>> predicate;
        Expression<Func<T, bool>> IQueryBuilder<T>.Expression
        {
            get
            {
                return predicate;
            }
            set
            {
                predicate = value;
            }
        }

        public QueryBuilder()
        {
            predicate = PredicateBuilder.True<T>();
        }
    }
}
