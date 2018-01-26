using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Core.Cache
{
    public static class CacheFactory
    {
        private static Dictionary<Type, object> lstM = new Dictionary<Type, object>();
        public static Cache<T> Instance<T>()
            where T : class
        {
            if (!lstM.Keys.Contains(typeof(T)))
            {
                lstM[typeof(T)] = new Cache<T>();
            }

            return (Cache<T>)lstM[typeof(T)];
        }
    }
}
