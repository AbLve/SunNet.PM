using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SF.Framework.Helpers;
using SF.Framework.Encrypt;
using SF.Framework.Encrypt.Providers;

namespace SF.Framework.Core
{
    public class SessionContext<T>
    {
        private static Dictionary<Type, SessionContext<T>> mappers = new Dictionary<Type, SessionContext<T>>();
        public static SessionContext<T> CurrentSession
        {
            get
            {
                Type t = typeof(T);
                if (!mappers.Keys.Contains(t))
                    mappers.Add(t, new SessionContext<T>());
                return mappers[t];
            }
        }

        private string key = string.Format("IdentityContext_{0}", typeof(T).ToString());

        public void Save(T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }
        public void Clear()
        {
            HttpContext.Current.Session.Remove(key);
        }
        public T Get()
        {
            return (T)HttpContext.Current.Session[key];
        }
    }
}
