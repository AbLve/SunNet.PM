using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Core.Validator;

namespace SunNet.PMNew.Framework.Core.UI.JS
{
    public abstract class JSValidatorHttpHandler<T, M> : IHttpHandler
        where T: class, new()
        where M : BaseValidator<T>
    {
        private M validator = null;

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            validator=InitValidator();
            context.Response.ContentType = "text/plain";
            string action = Convert.ToString(context.Request.QueryString["Action"])??"";
            action = action.Trim().ToLower();

            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();

            if (action == "GetPureRequester".ToLower())
            {
                T o = new T();
                InitEntity(o);
                context.Response.Write(json.Serialize(o));
            }
            else if (action == "Validate".ToLower())
            {
                T o = json.Deserialize<T>(context.Request.Form["entity"]);

                this.validator.Validate(o);

                context.Response.Write(json.Serialize(this.validator.BrokenRuleMessages));
            }
        }

        protected abstract M InitValidator();
        protected abstract void InitEntity(T o);
    }
}