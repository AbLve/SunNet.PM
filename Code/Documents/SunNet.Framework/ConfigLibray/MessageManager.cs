using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SF.Framework.ConfigLibray
{
    public class MessageManager
    {
        public static string ReadValueByKey(string key, string configPath = "Messages.xml")
        {
            string value = string.Empty;
            string filename = string.Empty;

            filename = System.AppDomain.CurrentDomain.BaseDirectory + configPath;


            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNode node = doc.SelectSingleNode("//MessageSettings");

            XmlElement element = (XmlElement)node.SelectSingleNode("//add[@key='" + key + "']");

            if (element != null)
            {
                value = element.GetAttribute("message");
            }

            return value;
        }
        public static string FormatValueByKey(string key, params object[] paras)
        {
            string value = string.Empty;
            string filename = string.Empty;
            string configPath = "Messages.xml";

            filename = System.AppDomain.CurrentDomain.BaseDirectory + configPath;


            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNode node = doc.SelectSingleNode("//MessageSettings");

            XmlElement element = (XmlElement)node.SelectSingleNode("//add[@key='" + key + "']");

            if (element != null)
            {
                value = element.GetAttribute("message");
            }
            string result = string.Format(value, paras);
            return result;
        }
    }
}
