using System;
using System.Collections.Generic;
using SF.Framework.Mvc.Search.Model;

namespace SF.Framework.Mvc.Search.TransformProviders
{
   
    public class DateBlockTransformProvider : ITransformProvider
    {
        public bool Match(ConditionItem item, Type type)
        {
            return item.Method == QueryMethod.DateBlock;
        }

        public IEnumerable<ConditionItem> Transform(ConditionItem item, Type type)
        {
            return new[]
                       {
                           new ConditionItem(item.Field, QueryMethod.GreaterThanOrEqual, item.Value),
                           new ConditionItem(item.Field, QueryMethod.LessThan, item.Value)
                       };
        }
    }
}