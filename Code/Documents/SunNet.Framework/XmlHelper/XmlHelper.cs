using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;
using SF.Framework.Log.Providers;

namespace SF.Framework.XmlHelper
{
    public class EmailTemplete
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class XmlHelper
    {
        public static string GetEmailContentToXmlByNode(string fileName, string node)
        {
            TextFileLogger log = new TextFileLogger();
            try
            {
                XmlDocument oXmlDoc = new XmlDocument();
                //  oXmlDoc.Load(SF.Framework.SFConfig.WebsitePhysicalPath + "/Content/EmailTemplate/" + fileName);
                //AppDomain.CurrentDomain.BaseDirectory
                string path = "";
                if (System.Web.HttpContext.Current != null)
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Content/EmailTemplate/" + fileName);
                else
                    path = ConfigurationManager.AppSettings["WebsitePath"] + "/Content/EmailTemplate/" + fileName;
                oXmlDoc.Load(path);
                XmlNode oNode = oXmlDoc.SelectSingleNode("/emails/email[position()=1]");
                string body = oNode.SelectSingleNode(node).InnerText;
                return body;
            }
            catch (Exception ex)
            {
                log.Log(ex);
                return "";
            }
        }
        public static EmailTemplete GetEmailTemplete(string fileName)
        {
            EmailTemplete t = new EmailTemplete();
            t.Subject = GetEmailContentToXmlByNode(fileName, "subject");
            t.Body = GetEmailContentToXmlByNode(fileName, "content");
            return t;
        }
    }
}
