using System;
using System.Web.Mvc;
using SF.Framework.Mvc.Search.Model;
using System.Linq;

namespace SF.Framework.Mvc.Search.Binders
{
    /// <summary>
    /// To SearchModel as Action parameters of binding
    /// </summary>
    public class SearchModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) 
        {
            var model = (QueryModel)(bindingContext.Model ?? new QueryModel());
            var dict = controllerContext.HttpContext.Request.Params;
            var keys = dict.AllKeys.Where(c => c.StartsWith("["));//Only beginning operation for '[' attributes
            if (keys.Count() != 0)
            {
                foreach (var key in keys)
                {
                    if (!key.StartsWith("[")) continue;
                    var val = dict[key];
                    //If is null or empty
                    if (string.IsNullOrEmpty(val)) continue;
                    AddSearchItem(model, key, val);
                }
            }
            return model; 
        }

        /// <summary>
        /// Will a group of key = value added to QueryModel.Items
        /// </summary>
        /// <param name="model">QueryModel</param>
        /// <param name="key">HtmlName</param>
        /// <param name="val">Value</param>
        public static void AddSearchItem(QueryModel model, string key, string val)
        {
            string field = "", prefix = "", orGroup = "", method = "";
            var keywords = key.Split(']', ')', '}');
            //The name will be Html divided into what we want several parts
            foreach (var keyword in keywords)
            {
                if (Char.IsLetterOrDigit(keyword[0])) field = keyword;
                var last = keyword.Substring(1);
                if (keyword[0] == '(') prefix = last;
                if (keyword[0] == '[') method = last;
                if (keyword[0] == '{') orGroup = last;       
            }
            if (string.IsNullOrEmpty(method)) return;
            if (!string.IsNullOrEmpty(field))
            {
                var item = new ConditionItem
                               {
                                   Field = field,
                                   Value = val.Trim(),
                                   Prefix = prefix,
                                   OrGroup = orGroup,
                                   Method = (QueryMethod) Enum.Parse(typeof (QueryMethod), method)
                               };
                model.Items.Add(item);
            }
        }
    }
}
