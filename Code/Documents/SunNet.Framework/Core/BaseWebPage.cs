using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace SF.Framework.Core
{
    public class BaseWebPage:Page
    {
        protected string QS_String(string parameterName)
        {
            return Convert.ToString(Request.QueryString[parameterName]);
        }

        protected Int32? QS_Int32(string parameterName)
        {
            string parameterValue = Request.QueryString[parameterName];

            if (parameterValue == null)
                return new Nullable<Int32>();

            if (parameterValue.Trim().Length == 0)
                throw new Exception(string.Format("Parameter [{0}] existed in querystring, but not assigned any value.", parameterName));

            Int32 parseResult;
            if (!Int32.TryParse(parameterValue.Trim(), out parseResult))
                throw new Exception(string.Format("Parameter [{0}]'s value [{1}] cannot be convert to Int32.", parameterName, parameterValue.Trim()));

            return parseResult;
        }

        protected Guid? QS_Guid(string parameterName)
        {
            string parameterValue = Request.QueryString[parameterName];

            if (parameterValue == null)
                return new Nullable<Guid>();

            if (parameterValue.Trim().Length == 0)
                throw new Exception(string.Format("Parameter [{0}] existed in querystring, but not assigned any value.", parameterName));

            Guid parseResult;
            if (!Guid.TryParse(parameterValue.Trim(), out parseResult))
                throw new Exception(string.Format("Parameter [{0}]'s value [{1}] cannot be convert to Guid.", parameterName, parameterValue.Trim()));

            return parseResult;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (IsRequireSSLConnection)
            {
                if (Request.RawUrl.Trim().ToLower().IndexOf("http://") == 0)
                {
                    string url = string.Format("https://{0}", Request.RawUrl.Trim().Substring(7));
                    Response.Redirect(url);
                }
            }
        }

        public virtual bool IsRequireSSLConnection
        {
            get
            {
                return false;
            }
        }
    }
}
