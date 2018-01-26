using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace SF.Framework.Mvc.Data.Expressions
{
    /// <summary>
    /// 动态查询条件创建者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueryBuilder<T>
    {
        Expression<Func<T, bool>> Expression { get; set; }
    }
}
