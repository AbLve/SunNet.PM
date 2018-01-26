using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.Caching;

#region Version Info
/* ========================================================================
*
* Author: lynnm Date: 2014/1/3 9:40:39
* File Name: FileAgent
* Version: 4.0.30319.1008
*
* ========================================================================
*/
#endregion
namespace FamilyBook.Common
{
    public class FileAgent
    {
        /// <summary>
        /// 构建文件存储路径(物理路径)，根据User ID获取存储位置
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string BuilderPhysicalDir(int userId)
        {
            var xml = HttpContext.Current.Cache["xml"] as XmlDocument;
            if (xml == null)
            {
                string xmlPath = HttpContext.Current.Server.MapPath("~/distributedmemory.xml");
                xml = new XmlDocument();
                xml.Load(xmlPath);
                //缓存键依赖项。当文档内容更改时，该对象即无效，并从缓存中移除。 
                CacheDependency c = new CacheDependency(xmlPath);
                HttpContext.Current.Cache.Insert("xml", xml, c, Cache.NoAbsoluteExpiration, System.TimeSpan.Zero);
            }
            XmlNode oNode = xml.SelectSingleNode("/memorys/memory[startId<=" + userId + "][endId>=" + userId + "]");
            string url = oNode.SelectSingleNode("physical").InnerText;
            return url;
        }

        /// <summary>
        /// 构建文件读取路径network
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string BuilderVirtualDir(int userId)
        {
            var xml = HttpContext.Current.Cache["xml"] as XmlDocument;
            if (xml == null)
            {
                string xmlPath = HttpContext.Current.Server.MapPath("~/distributedmemory.xml");
                xml = new XmlDocument();
                xml.Load(xmlPath);
                //缓存键依赖项。当文档内容更改时，该对象即无效，并从缓存中移除。 
                CacheDependency c = new CacheDependency(xmlPath);
                HttpContext.Current.Cache.Insert("xml", xml, c, Cache.NoAbsoluteExpiration, System.TimeSpan.Zero);
            }
            XmlNode oNode = xml.SelectSingleNode("/memorys/memory[startId<=" + userId + "][endId>=" + userId + "]");
            string url = oNode.SelectSingleNode("network").InnerText;
            return url;
        }
    }
}
