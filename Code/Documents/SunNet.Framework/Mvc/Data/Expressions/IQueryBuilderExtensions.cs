using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Linq.SqlClient;
using System.Collections.Generic;

namespace SF.Framework.Mvc.Data.Expressions
{
    public static class IQueryBuilderExtensions
    {
        /// <summary>
        /// Establish Between query conditions
        /// </summary>
        /// <typeparam name="T">entity</typeparam>
        /// <param name="q">Dynamic query conditions creator</param>
        /// <param name="property">property</param>
        /// <param name="from">begin value</param>
        /// <param name="to">end value</param>
        /// <returns></returns>
        public static IQueryBuilder<T> Between<T, P>(this IQueryBuilder<T> q, Expression<Func<T, P>> property, P from, P to)
        {
            var parameter = property.GetParameters();
            var constantFrom = Expression.Constant(from);
            var constantTo = Expression.Constant(to);
            Type type = typeof(P);
            Expression nonNullProperty = property.Body;
            //If it is Nullable<X> type, then into X type
            if (IsNullableType(type))
            {
                type = GetNonNullableType(type);
                nonNullProperty = Expression.Convert(property.Body, type);
            }
            var c1 = Expression.GreaterThanOrEqual(nonNullProperty, constantFrom);
            var c2 = Expression.LessThanOrEqual(nonNullProperty, constantTo);
            var c = Expression.AndAlso(c1, c2);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(c, parameter);

            q.Expression = q.Expression.And(lambda);
            return q;
        }

        /// <summary>
        /// Establish Like (fuzzy) inquires the condition
        /// </summary>
        /// <typeparam name="T">entity</typeparam>
        /// <param name="q">Dynamic query conditions creator</param>
        /// <param name="property">property</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static IQueryBuilder<T> Like<T>(this IQueryBuilder<T> q, Expression<Func<T, string>> property, string value)
        {
            value = value.Trim();
            if (!string.IsNullOrEmpty(value))
            {
                var parameter = property.GetParameters();
                var constant = Expression.Constant("%" + value + "%");
                MethodCallExpression methodExp = Expression.Call(null, typeof(SqlMethods).GetMethod("Like",
                    new Type[] { typeof(string), typeof(string) }), property.Body, constant);
                Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(methodExp, parameter);

                q.Expression = q.Expression.And(lambda);
            }
            return q;
        }

        /// <summary>
        /// Establish Equals (equal) inquires the condition
        /// </summary>
        /// <typeparam name="T">entity</typeparam>
        /// <param name="q">Dynamic query conditions creator</param>
        /// <param name="property">property</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static IQueryBuilder<T> Equals<T, P>(this IQueryBuilder<T> q, Expression<Func<T, P>> property, P value)
        {
            var parameter = property.GetParameters();
            var constant = Expression.Constant(value);
            Type type = typeof(P);
            Expression nonNullProperty = property.Body;
            
            if (IsNullableType(type))
            {
                type = GetNonNullableType(type);
                nonNullProperty = Expression.Convert(property.Body, type);
            }
            var methodExp = Expression.Equal(nonNullProperty, constant);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(methodExp, parameter);
            q.Expression = q.Expression.And(lambda);
            return q;
        }

        /// <summary>
        /// Establish In inquires the condition
        /// </summary>
        /// <typeparam name="T">entity</typeparam>
        /// <param name="q">Dynamic query conditions creator</param>
        /// <param name="property">property</param>
        /// <param name="values">values</param>
        /// <returns></returns>
        public static IQueryBuilder<T> In<T, P>(this IQueryBuilder<T> q, Expression<Func<T, P>> property, params P[] values)
        {
            if (values != null && values.Length > 0)
            {
                var parameter = property.GetParameters();
                var constant = Expression.Constant(values);
                Type type = typeof(P);
                Expression nonNullProperty = property.Body;
                
                if (IsNullableType(type))
                {
                    type = GetNonNullableType(type);
                    nonNullProperty = Expression.Convert(property.Body, type);
                }
                Expression<Func<P[], P, bool>> InExpression = (list, el) => list.Contains(el);
                var methodExp = InExpression;
                var invoke = Expression.Invoke(methodExp, constant, property.Body);
                Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(invoke, parameter);
                q.Expression = q.Expression.And(lambda);
            }
            return q;
        }

        private static ParameterExpression[] GetParameters<T, S>(this Expression<Func<T, S>> expr)
        {
            return expr.Parameters.ToArray();
        }

        static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        static Type GetNonNullableType(Type type)
        {
            return type.GetGenericArguments()[0];
            //return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
        }


    }
}
