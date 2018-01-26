using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SF.Framework.Mvc.Search.Model;
using SF.Framework.Mvc.Search.TransformProviders;
namespace SF.Framework.Mvc.Search
{
    internal class QueryableSearcher<T>
    {
        public QueryableSearcher()
        {
            Init();
        }
        public QueryableSearcher(IQueryable<T> table, IEnumerable<ConditionItem> items)
            : this()
        {
            Table = table;
            Items = items;
        }
        private void Init()
        {
            TransformProviders = new List<ITransformProvider>
                                     {
                                         new LikeTransformProvider(),
                                         new DateBlockTransformProvider(),
                                         new InTransformProvider(),
                                         new UnixTimeTransformProvider()
                                     };
        }



        public List<ITransformProvider> TransformProviders { get; set; }
        protected IEnumerable<ConditionItem> Items { get; set; }

        protected IQueryable<T> Table { get; set; }

        public IQueryable<T> Search()
        {
            //Construct lambda parameters
            ParameterExpression param = Expression.Parameter(typeof(T), "c");
            //Construct body for lambda parameters
            var body = GetExpressoinBody(param, Items);
            //Construct lambda
            var expression = Expression.Lambda<Func<T, bool>>(body, param);
            //Return to Expression<Func<T,bool>>
            return Table.Where(expression);
        }

        private Expression GetExpressoinBody(ParameterExpression param, IEnumerable<ConditionItem> items)
        {
            var list = new List<Expression>();
            //OrGroup is empty, that is for And combination
            var andList = items.Where(c => string.IsNullOrEmpty(c.OrGroup));
            //Will the son And Expression to AndAlso splicing
            if (andList.Count() != 0)
            {
                list.Add(GetGroupExpression(param, andList, Expression.AndAlso));
            }
            //The other is Or relationship, between different Or group with And separation
            var orGroupByList = items.Where(c => !string.IsNullOrEmpty(c.OrGroup)).GroupBy(c => c.OrGroup);
            //The son of joining together of Expression Or relationship
            foreach (IGrouping<string, ConditionItem> group in orGroupByList)
            {
                if (group.Count() != 0)
                    list.Add(GetGroupExpression(param, group, Expression.OrElse));
            }
            //Will these Expression again with And linked
            return list.Aggregate(Expression.AndAlso);
        }

        private Expression GetGroupExpression(ParameterExpression param, IEnumerable<ConditionItem> items, Func<Expression, Expression, Expression> func)
        {
            //Obtaining the minimal judgment expression
            var list = items.Select(item => GetExpression(param, item));
            //Again with logic operator connected
            return list.Aggregate(func);
        }

        private Expression GetExpression(ParameterExpression param, ConditionItem item)
        {
            //Attribute expression
            LambdaExpression exp = GetPropertyLambdaExpression(item, param);
            //If there are special type processing, the processing, temporary no attention
            foreach (var provider in TransformProviders)
            {
                if (provider.Match(item, exp.Body.Type))
                {
                    return GetGroupExpression(param, provider.Transform(item, exp.Body.Type), Expression.AndAlso);
                }
            }
            //Constant expression
            var constant = ChangeTypeToExpression(item, exp.Body.Type);
            //Judged by the operator or method connection
            return ExpressionDict[item.Method](exp.Body, constant);
        }

        private LambdaExpression GetPropertyLambdaExpression(ConditionItem item, ParameterExpression param)
        {
            //For every level attributes such as c.Users.Proiles.UserId
            var props = item.Field.Split('.');
            Expression propertyAccess = param;
            var typeOfProp = typeof(T);
            int i = 0;
            do
            {
                PropertyInfo property = typeOfProp.GetProperty(props[i]);
                if (property == null) return null;
                typeOfProp = property.PropertyType;
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                i++;
            } while (i < props.Length);

            return Expression.Lambda(propertyAccess, param);
        }

        #region ChangeType

        /// <summary>
        /// Type conversion, support the empty types and nullable type conversion between
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversionType)
        {
            if (value == null) return null;
            return Convert.ChangeType(value, TypeUtil.GetUnNullableType(conversionType));
        }

        /// <summary>
        /// SearchItem conversion of the type of Value for expression tree
        /// </summary>
        /// <param name="item"></param>
        /// <param name="conversionType">Return type</param>
        public static Expression ChangeTypeToExpression(ConditionItem item, Type conversionType)
        {
            if (item.Value == null) return Expression.Constant(item.Value, conversionType);
            #region Array
            if (item.Method == QueryMethod.StdIn)
            {
                var arr = (item.Value as Array);
                var expList = new List<Expression>();
                //It's useable
                if (arr != null)
                    for (var i = 0; i < arr.Length; i++)
                    {
                        //Structure of the array element Constant
                        var newValue = ChangeType(arr.GetValue(i), conversionType);
                        expList.Add(Expression.Constant(newValue, conversionType));
                    }
                //Structure inType types of array expression tree, and for array initialization
                return Expression.NewArrayInit(conversionType, expList);
            }

            #endregion

            var elementType = TypeUtil.GetUnNullableType(conversionType);
            var value = Convert.ChangeType(item.Value, elementType);
            return Expression.Constant(value, conversionType);
        }

        #endregion

        #region SearchMethod Operation Method

        private static readonly Dictionary<QueryMethod, Func<Expression, Expression, Expression>> ExpressionDict =
            new Dictionary<QueryMethod, Func<Expression, Expression, Expression>>
                {
                    {
                        QueryMethod.Equal,
                        (left, right) => { return Expression.Equal(left, right); }
                        },
                    {
                        QueryMethod.GreaterThan,
                        (left, right) => { return Expression.GreaterThan(left, right); }
                        },
                    {
                        QueryMethod.GreaterThanOrEqual,
                        (left, right) => { return Expression.GreaterThanOrEqual(left, right); }
                        },
                    {
                        QueryMethod.LessThan,
                        (left, right) => { return Expression.LessThan(left, right); }
                        },
                    {
                        QueryMethod.LessThanOrEqual,
                        (left, right) => { return Expression.LessThanOrEqual(left, right); }
                        },
                    {
                        QueryMethod.Contains,
                        (left, right) =>
                            {
                                if (left.Type != typeof (string)) return null;
                                return Expression.Call(left, typeof (string).GetMethod("Contains"), right);
                            }
                        },
                    {
                        QueryMethod.StdIn,
                        (left, right) =>
                            {
                                if (!right.Type.IsArray) return null;
                                //Call Enumerable. Contains extension methods
                                MethodCallExpression resultExp =
                                    Expression.Call(
                                        typeof (Enumerable),
                                        "Contains",
                                        new[] {left.Type},
                                        right,
                                        left);

                                return resultExp;
                            }
                        },
                    {
                        QueryMethod.NotEqual,
                        (left, right) => { return Expression.NotEqual(left, right); }
                        },
                    {
                        QueryMethod.StartsWith,
                        (left, right) =>
                            {
                                if (left.Type != typeof (string)) return null;
                                return Expression.Call(left, typeof (string).GetMethod("StartsWith", new[] {typeof (string)}), right);

                            }
                        },
                    {
                        QueryMethod.EndsWith,
                        (left, right) =>
                            {
                                if (left.Type != typeof (string)) return null;
                                return Expression.Call(left, typeof (string).GetMethod("EndsWith", new[] {typeof (string)}), right);
                            }
                        },
                    {
                        QueryMethod.DateTimeLessThanOrEqual,
                        (left, right) => { return Expression.LessThanOrEqual(left, right); }
                        }
                };

        #endregion
    }
}