using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace SunNet.PMNew.Framework.Utils.Helpers
{
    public class XMLSerializerHelper
    {
        public  object XmlDeserializerFormFile(Type type, string path)
        {
            return new XmlSerializer(type).Deserialize(new XmlTextReader(path));
        }


        public  object XmlDeserializerFormText(Type type, string serializeText)
        {
            using (StringReader reader = new StringReader(serializeText))
            {
                return new XmlSerializer(type).Deserialize(reader);
            }
        }

        public  void XmlSerializerToFile(object target, string path)
        {
            StreamWriter writer = new StreamWriter(path);
            new XmlSerializer(target.GetType()).Serialize((StreamWriter)writer, target);
            writer.Close();
        }


        public  string XmlSerializerToXml(object target)
        {
            return XmlSerializerToText(target, false);
        }


        public  string XmlSerializerToText(object target)
        {
            return XmlSerializerToText(target, true);
        }


        private  string XmlSerializerToText(object target, bool isText)
        {
            StringWriter writer = new StringWriter();
            new XmlSerializer(target.GetType()).Serialize((TextWriter)writer, target);
            StringBuilder sb = writer.GetStringBuilder();
            writer.Close();
            if (isText)
            {
                sb.Replace("<?xml  version=\"1.0\"  encoding=\"utf-16\"?>\r\n", "");
                sb.Replace("  xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            }
            else
            {
                sb.Replace("utf-16", "utf-8");
            }
            return sb.ToString();
        }
    }
}
