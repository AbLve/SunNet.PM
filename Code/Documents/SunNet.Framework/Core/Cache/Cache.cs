using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Cache;

namespace SF.Framework.Core.Cache
{
    public class Cache<T> where T : class
    {
        private string prefix4M = typeof(T).GetType().ToString();

        private ICache cache = SFConfig.Components.Cache(typeof(T));

        protected string GetCacheKeyM(Guid id)
        {
            return GetCacheKeyM(id.ToString());
        }
        protected string GetCacheKeyM(string key)
        {
            key = string.Format("{0}::Detail::{1}", prefix4M, key);
            return key;
        }

        

        public void AddCache(Guid id, T o)
        {
            string key = GetCacheKeyM(id);
            cache[key] = o;
        }
        public void UpdateCache(Guid id, T o)
        {
            string key = GetCacheKeyM(id);
            if (cache[key] == null)
                throw new Exception("Not cached object.");
            cache[key] = o;
        }
        public void RemoveCache(Guid id)
        {
            string key = GetCacheKeyM(id);
            if (cache[key] == null)
                throw new Exception("Not cached object.");
            cache[key] = null;
        }


        public T GetByID(Guid id)
        {
            return (T)cache[GetCacheKeyM(id)];
        }

        public object GetByKey(string key)
        {
            key = GetCacheKeyM(key);
            return cache[key];
        }
        public void SetByKey(string key, object o)
        {
            key = GetCacheKeyM(key);
            cache[key] = o;
        }
    }
}
