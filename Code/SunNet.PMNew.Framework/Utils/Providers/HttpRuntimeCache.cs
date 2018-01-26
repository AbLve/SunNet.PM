using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace SunNet.PMNew.Framework.Utils.Providers
{
    public class HttpRuntimeCache<T> : ICache<T>
    {
        private string GetKey(string key)
        {
            return string.Format("{0}::{1}", typeof(T).ToString().ToLower().Trim(), key.ToLower().Trim());
        }

        public object this[string key]
        {
            get
            {
                string realKey = GetKey(key);
                return HttpRuntime.Cache[realKey];
            }
            set
            {
                string realKey = GetKey(key);

                if (value == null)
                {
                    HttpRuntime.Cache.Remove(realKey);
                    return;
                }

                HttpRuntime.Cache[realKey] = value;
            }
        }
    }
}