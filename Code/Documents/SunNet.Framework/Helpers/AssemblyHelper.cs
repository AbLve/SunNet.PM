using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SF.Framework.Helpers
{
    public static class AssemblyHelper
    {
        public static Type CreateType(string dllName, string className)
        {
            Assembly assembly = Assembly.Load(dllName);
            Type type=assembly.GetType(className);
            return type;
        }

        public static T CreateNew<T>(string dllName, string className) where T:class
        {
            T o = Assembly.Load(dllName).CreateInstance(className) as T;
            return o;
        }

        public static object CreateNew(string dllName, string className)
        {
            return Assembly.Load(dllName).CreateInstance(className);
        }
    }
}
