
using System;
using System.Collections.Generic;
using SF.Framework.Mvc.Search.Model;

namespace SF.Framework.Mvc.Search.TransformProviders
{
    

    public interface ITransformProvider
    {
        bool Match(ConditionItem item, Type type);
        IEnumerable<ConditionItem> Transform(ConditionItem item, Type type);
    }
}