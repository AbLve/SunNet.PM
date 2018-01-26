
using System.Collections.Generic;
namespace SF.Framework.Mvc.Search.Model
{
    /// <summary>
    /// Dynamic query an object
    /// </summary>
    public class QueryModel
    {
        public QueryModel()
        {
            Items = new List<ConditionItem>();
        }
        /// <summary>
        /// Query items
        /// </summary>
        public List<ConditionItem> Items { get; set; }     

    }
}