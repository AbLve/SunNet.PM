
namespace SF.Framework.Mvc.Search.Model
{
    /// <summary>
    /// Used for storage inquires the conditions of the unit
    /// </summary>
    public class ConditionItem
    {
        public ConditionItem(){}

        public ConditionItem(string field, QueryMethod method, object val)
        {
            Field = field;
            Method = method;
            Value = val;
        }

        /// <summary>
        /// Field
        /// </summary>
        public string Field { get; set; }
        
        /// <summary>
        /// Inquires the way, used to mark inquires the way used in HtmlName [] for identification
        /// </summary>
        public QueryMethod Method { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The prefix, used to mark scope, used in HTMLName () for identification
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// If you use Or combination, the combination of group as a Or sequence
        /// </summary>
        public string OrGroup { get; set; }
    }
}
