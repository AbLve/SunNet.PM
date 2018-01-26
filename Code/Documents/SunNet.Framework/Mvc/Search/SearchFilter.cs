using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Mvc.Search
{
    public class SearchFilter
    {
        public static SF.Framework.Mvc.Search.Model.QueryModel GetGeneralResult(SF.Framework.Mvc.Search.Model.QueryModel model)
        {
            if (model != null)
            {
                foreach (var item in model.Items)
                {
                    if (item.Method == Model.QueryMethod.Like)
                    {
                        item.Value = "*" + item.Value + "*";
                    }
                    //note: StdIn Method,Convert item.Value to array
                    //else if (item.Method == Model.QueryMethod.StdIn)
                    //{
                    //    item.Value = item.Value.ToString().Split(',');
                    //}
                }
            }
            return model;
        }
    }
}
