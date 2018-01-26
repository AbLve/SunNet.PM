using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SF.Framework.Log;

namespace SF.Framework.ConfigLibray
{
    public class ConfigManager
    {
        public ConfigManager()
        { }

        public static string ReadValueByKey(string key, string configPath = "web.config")
        {
            string value = string.Empty;
            string filename = string.Empty;

            filename = System.AppDomain.CurrentDomain.BaseDirectory + configPath;


            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNode node = doc.SelectSingleNode("//appSettings");

            XmlElement element = (XmlElement)node.SelectSingleNode("//add[@key='" + key + "']");

            if (element != null)
            {
                value = element.GetAttribute("value");
            }

            return value;
        }

        public static string ReadConnectionStringByName(string name, string configPath = "web.config")
        {
            string connectionString = string.Empty;
            string filename = string.Empty;

            filename = System.AppDomain.CurrentDomain.BaseDirectory + configPath;


            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNode node = doc.SelectSingleNode("//connectionStrings");

            XmlElement element = (XmlElement)node.SelectSingleNode("//add[@name='" + name + "']");

            if (element != null)
            {
                connectionString = element.GetAttribute("connectionString");
            }

            return connectionString;
        }

        public static bool UpdateOrCreateAppSetting(string key, string value, string configPath = "web.config")
        {
            bool isSuccess = false;
            string filename = string.Empty;

            filename = System.AppDomain.CurrentDomain.BaseDirectory + configPath;


            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNode node = doc.SelectSingleNode("//appSettings");

            try
            {
                XmlElement element = (XmlElement)node.SelectSingleNode("//add[@key='" + key + "']");

                if (element != null)
                {
                    element.SetAttribute("value", value);
                }
                else
                {
                    XmlElement subElement = doc.CreateElement("add");
                    subElement.SetAttribute("key", key);
                    subElement.SetAttribute("value", value);
                    node.AppendChild(subElement);
                }

                using (XmlTextWriter xmlwriter = new XmlTextWriter(filename, null))
                {
                    xmlwriter.Formatting = Formatting.Indented;
                    doc.WriteTo(xmlwriter);
                    xmlwriter.Flush();
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                isSuccess = false;
            }

            return isSuccess;
        }

        public static bool UpdateOrCreateConnectionString(string name, string connectionString, string providerName, string configPath = "web.config")
        {
            bool isSuccess = false;
            string filename = string.Empty;

            filename = System.AppDomain.CurrentDomain.BaseDirectory + configPath;


            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNode node = doc.SelectSingleNode("//connectionStrings");

            try
            {
                XmlElement element = (XmlElement)node.SelectSingleNode("//add[@name='" + name + "']");

                if (element != null)
                {
                    element.SetAttribute("connectionString", connectionString);
                    element.SetAttribute("providerName", providerName);
                }
                else
                {
                    XmlElement subElement = doc.CreateElement("add");
                    subElement.SetAttribute("name", name);
                    subElement.SetAttribute("connectionString", connectionString);
                    subElement.SetAttribute("providerName", providerName);
                    node.AppendChild(subElement);
                }

                doc.Save(filename);

                isSuccess = true;
            }
            catch (Exception e)
            {
                WebLogAgent.Write(e);
                isSuccess = false;
            }

            return isSuccess;
        }

        public static bool RemoveByKey(string key, string configPath = "web.config")
        {
            bool isSuccess = false;
            string filename = string.Empty;

            filename = System.AppDomain.CurrentDomain.BaseDirectory + configPath;


            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNode node = doc.SelectSingleNode("//appSettings");

            XmlElement element = (XmlElement)node.SelectSingleNode("//add[@key='" + key + "']");

            if (element != null)
            {
                element.ParentNode.RemoveChild(element);
            }
            else
            {
            }

            try
            {
                using (XmlTextWriter xmlwriter = new XmlTextWriter(filename, null))
                {
                    xmlwriter.Formatting = Formatting.Indented;
                    doc.WriteTo(xmlwriter);
                    xmlwriter.Flush();
                }

                isSuccess = true;
            }
            catch (Exception e)
            {
                WebLogAgent.Write(e);
                isSuccess = false;
                //Output string for debug
                //string strOuput = string.Format("The node cannot be remove[appSettings]. Key:{0} Error:{1}\n", key, e.Message);
                //Write in the file of log.
                //Coding.
            }

            return isSuccess;
        }

        public static bool RemoveByName(string name, string configPath = "web.config")
        {
            bool isSuccess = false;
            string filename = string.Empty;

            filename = System.AppDomain.CurrentDomain.BaseDirectory + configPath;


            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNode node = doc.SelectSingleNode("//connectionStrings");

            XmlElement element = (XmlElement)node.SelectSingleNode("//add[@name='" + name + "']");

            if (element != null)
            {
                node.RemoveChild(element);
            }
            else
            {
            }

            try
            {
                doc.Save(filename);

                isSuccess = true;
            }
            catch (Exception e)
            {
                WebLogAgent.Write(e);
                isSuccess = false;
                //Output string for debug
                //string strOuput = string.Format("The node cannot be remove[appSettings]. Key:{0} Error:{1}\n", key, e.Message);
                //Write in the file of log.
                //Coding.
            }

            return isSuccess;
        }

    }
}
