using System;
using System.Collections.Generic;
using SF.Framework.Mvc.Search.Model;

namespace SF.Framework.Mvc.Search.TransformProviders
{
    

    public class InTransformProvider:ITransformProvider
    {
        public bool Match(ConditionItem item, Type type)
        {
            return item.Method == QueryMethod.In;
        }

        public IEnumerable<ConditionItem> Transform(ConditionItem item, Type type)
        {
            var arr = (item.Value as Array);
            if (arr == null)
            {
                var arrStr = item.Value.ToString();
                if (!string.IsNullOrEmpty(arrStr))
                {
                    arr = arrStr.Split(',');
                }
            }
            return new[] { new ConditionItem(item.Field, QueryMethod.StdIn, arr) };
        }
    }
}