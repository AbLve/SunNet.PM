using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using System.Web;

namespace SF.Framework.Helpers
{
    public class IoCInjection
    {
        public class InjectionItem
        {
            public string RequestClassName { get; set; }
            public string RequestDllName { get; set; }
            public string ConcreteDllName { get; set; }
            public string ConcreteClassName { get; set; }
        }

        public static void LoadInjectionFromXmlFile(string xmlPath, ConfigurationExpression x)
        {

            List<InjectionItem> injectionItems = XMLSerializerHelper.XmlDeserializerFormFile(typeof(List<InjectionItem>), xmlPath) as List<InjectionItem>;
            foreach (InjectionItem item in injectionItems)
            {
                Type type = AssemblyHelper.CreateType(item.RequestDllName, item.RequestClassName);
                object o = AssemblyHelper.CreateNew(item.ConcreteDllName, item.ConcreteClassName);
                x.For(type).Singleton().Use(o);
            }
        }
    }
}