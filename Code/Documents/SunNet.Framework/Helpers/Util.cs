using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web;
using System.IO;
using System.Xml.Xsl;
using System.Xml.XPath;
using SF.Framework.Log;

namespace SF.Framework.Helpers
{
    public class Util
    {
        public static IList EnumToListStr(Type enumType)
        {
            ArrayList list = new ArrayList();

            for (int i = 0; i < Enum.GetValues(enumType).Length; i++)
            {
                ListItem listitem = new ListItem(Enum.GetName(enumType, i), Enum.GetName(enumType, i));
                list.Add(listitem);
            }
            return list;
        }

        public static IList EnumToListInt(Type enumType)
        {
            ArrayList list = new ArrayList();

            foreach (int i in Enum.GetValues(enumType))
            {
                ListItem listitem = new ListItem(Enum.GetName(enumType, i), i.ToString());
                list.Add(listitem);
            }
            return list;
        }
        public static IList EnumToListInt(Type enumType, string NeedRemoveSpecificSymbol)
        {
            ArrayList list = new ArrayList();

            foreach (int i in Enum.GetValues(enumType))
            {
                ListItem listitem = new ListItem(Enum.GetName(enumType, i).Replace(NeedRemoveSpecificSymbol, " "), i.ToString());
                list.Add(listitem);
            }
            return list;
        }
        public static void DownLoadExcel(HttpResponse response, string xml, string xsltpath, string filename)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XPathNavigator navgator = doc.CreateNavigator();
                StringWriter output = new StringWriter();
                XslCompiledTransform transform = new XslCompiledTransform();
                transform.Load(xsltpath, new XsltSettings(false, true), new XmlUrlResolver());
                transform.Transform(navgator, null, output);
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                response.AddHeader("Content-Transfer-Encoding", "binary");
                response.ContentType = "ms-excel";
                response.Write(output.ToString().Replace("\r\n", "\r"));
                response.Flush();
            }
            catch (Exception ex)
            {
                new log4netProvider().Log(ex);
            }
            finally
            {
                response.End();
            }
        }
    }
}
