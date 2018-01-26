using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace SF.Framework.Cache.Providers
{
    public class HttpRuntimeCache : ICache
    {
        private string cachePrefix = string.Empty;
        private string GetKey(string key)
        {
            return string.Format("{0}::{1}", this.cachePrefix.ToLower().Trim(), key.ToLower().Trim());
        }

        public object this[string key]
        {
            get
            {
                AssurePrefixExists();

                string realKey = GetKey(key);
                return HttpRuntime.Cache[realKey];
            }
            set
            {
                AssurePrefixExists();

                string realKey = GetKey(key);

                if (value == null)
                {
                    HttpRuntime.Cache.Remove(realKey);
                    return;
                }

                HttpRuntime.Cache[realKey] = value;
            }
        }

        private void AssurePrefixExists()
        {
            if (this.cachePrefix == null || this.cachePrefix.Trim().Length == 0)
                throw new Exception("Cache prefix must be specifiy.");
        }

        public string CachePrefix
        {
            get
            {
                return this.cachePrefix;
            }
            set
            {
                this.cachePrefix = value;
            }
        }
    }
}