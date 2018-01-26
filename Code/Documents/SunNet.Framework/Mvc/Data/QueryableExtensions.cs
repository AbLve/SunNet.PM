using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace SF.Framework.Mvc.Data
{

    public static class QueryableExtensions
    {
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName)
        {
            return QueryableHelper<T>.OrderBy(queryable, propertyName, false);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName, bool desc)
        {
            return QueryableHelper<T>.OrderBy(queryable, propertyName, desc);
        }

        public static IOrderedQueryable<T> ThanBy<T>(this IOrderedQueryable<T> queryable, string propertyName, bool desc)
        {
            return QueryableHelper<T>.ThanBy(queryable, propertyName, desc);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, Dictionary<string, bool> propertyNames)
        {
            IOrderedQueryable<T> output = null;
            foreach (var item in propertyNames)
            {
                if (output == null)
                    output = QueryableHelper<T>.OrderBy(queryable, item.Key, item.Value);
                else
                {
                    output = output.ThanBy(item.Key, item.Value);
                }
            }
            return output ?? queryable;
        } 
      
        static class QueryableHelper<T>
        {

            private static Dictionary<string, LambdaExpression> cache = new Dictionary<string, LambdaExpression>();

            public static IOrderedQueryable<T> OrderBy(IQueryable<T> queryable, string propertyName, bool desc)
            {
                dynamic keySelector = GetLambdaExpression(propertyName);
                return desc ? Queryable.OrderByDescending(queryable, keySelector) : Queryable.OrderBy(queryable, keySelector);
            }

            public static IOrderedQueryable<T> ThanBy(IOrderedQueryable<T> queryable, string propertyName, bool desc)
            {
                dynamic keySelector = GetLambdaExpression(propertyName); 
                return desc ? Queryable.ThenByDescending(queryable, keySelector) : Queryable.ThenBy(queryable, keySelector);
            }

            private static LambdaExpression GetLambdaExpression(string propertyName)
            { 
                if (cache.ContainsKey(propertyName)) return cache[propertyName];
                var param = Expression.Parameter(typeof(T));
                var body = PropertyOfProperty(param, propertyName);
                var keySelector = Expression.Lambda(body, param);
                cache[propertyName] = keySelector;
                return keySelector;
            }

            private static MemberExpression PropertyOfProperty(Expression expr, string propertyName)
            {
                return propertyName.Split('.').Aggregate<string, MemberExpression>(null, (current, property) => Expression.Property(current ?? expr, property));
            }
        }
    }

}
