using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SF.Framework.Mvc.Search.Model;

namespace SF.Framework.Mvc.Search
{
    

    /// <summary>
    /// IQueryable to the extension methods
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Make IQueryable support QueryModel
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="table">IQueryable Query Object</param>
        /// <param name="model">QueryModel Object</param>
        /// <param name="prefix">Use the prefix distinguish inquires the condition</param>
        /// <returns></returns>
        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> table, QueryModel model, string prefix = "") where TEntity : class
        {
            Contract.Requires(table != null);
            return Where<TEntity>(table, model.Items, prefix);
        }

        private static IQueryable<TEntity> Where<TEntity>(IQueryable<TEntity> table, IEnumerable<ConditionItem> items, string prefix = "")
        {
            Contract.Requires(table != null);
            IEnumerable<ConditionItem> filterItems =
                string.IsNullOrWhiteSpace(prefix)
                    ? items.Where(c => string.IsNullOrEmpty(c.Prefix))
                    : items.Where(c => c.Prefix == prefix);
            if (filterItems.Count() == 0) return table;
            return new QueryableSearcher<TEntity>(table, filterItems).Search();
        }
    }
}
