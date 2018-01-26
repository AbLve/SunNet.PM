using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Mvc.Pager;

namespace SF.Framework
{
    public class PagedJsonResult
    {
        /// <summary>
        /// Create json for PagedList result.
        /// </summary>
        /// <param name="pagedList">PagedList object.</param>
        /// <param name="dic">Expand result.</param>
        /// <returns>Json string result.</returns>
        public static string ToPagedJson<T>(PagedList<T> pagedList, Dictionary<string, object> dic = null)
        {
            var result = new Dictionary<string, object>();
            result.Add("page", pagedList == null ? 0 : pagedList.CurrentPageIndex);
            result.Add("total", pagedList == null ? 0 : pagedList.TotalItemCount);
            result.Add("rows", pagedList == null ? null : pagedList.ToArray());
            if (dic != null && dic.Count > 0)
            {
                foreach (var item in dic)
                {
                    result.Add(item.Key, item.Value);
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
    }
}
