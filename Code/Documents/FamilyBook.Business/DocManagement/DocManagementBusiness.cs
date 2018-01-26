using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyBook.Core.DocManagementModule;
using FamilyBook.Core;
using FamilyBook.Entity.DocManagements;
using SF.Framework.StringZipper.Providers;
using SF.Framework.Utils.Helpers;
using System.Transactions;
using SF.Framework.Log;
using SF.Framework.File;
using FamilyBook.Common;
using System.Text.RegularExpressions;
using System.Web;

#region Version Info
/* ========================================================================
*
* Author: lynnm Date: 2013/12/2 10:09:57
* File Name: DocManagementBusiness
* Version: 4.0.30319.1008
*
* ========================================================================
*/
#endregion
namespace FamilyBook.Business.DocManagement
{
    public class DocManagementBusiness
    {
        private DocManagementService service = DomainFacade.CreateDocManagementService();

        public int Insert(DocManagementEntity entity)
        {
            return service.Insert(entity);
        }

        /// <summary>
        /// 上传多个文件 保存上传记录
        /// </summary>
        /// <param name="listEntity"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool InsertList(int projectId, List<SF.Framework.File.FileEntity> listEntity, int parentId, int userId, int companyID)
        {
#if !DEBUG
            using (TransactionScope scope = new TransactionScope())
#endif
            {
                try
                {
                    foreach (var item in listEntity)
                    {
                        DocManagementEntity entity = new DocManagementEntity();
                        entity.ProjectID = projectId;
                        entity.CompanyID = companyID;
                        entity.UserID = userId;
                        entity.Type = Entity.DocManagements.DocType.File;
                        entity.ParentID = parentId;
                        entity.FileUrl = item.FilePath;
                        entity.FileContentType = item.ContentType;
                        entity.FileSize = item.Size;
                        entity.FileName = item.DbName;
                        entity.DisplayFileName = item.DisplayName;
                        entity.Extenstions = item.Extension;
                        Insert(entity);
                    }
                }
                catch (Exception ex)
                {
                    new log4netProvider().Log(ex);
                    return false;
                }
#if !DEBUG
                scope.Complete();
#endif
                return true;
            }
        }

        public bool Delete(int projectId, int id)
        {
#if !DEBUG
            using (TransactionScope scope = new TransactionScope())
#endif
            {
                bool isSucess = true;
                try
                {
                    int totaoFileSize = 0;
                    List<DocManagementEntity> list = GetListAllChildByParentId(id, service.GetList(projectId, ""));
                    foreach (var item in list)
                    {
                        totaoFileSize += item.FileSize;
                        FileHelper.Delete(SF.Framework.SFConfig.FilePhysicalUrl + item.FileUrl);
                        isSucess = service.Delete(projectId, item.ID);
                    }
                    DocManagementEntity docEntity = service.GetEntity(id);
                    totaoFileSize += docEntity.FileSize;
                    FileHelper.Delete(SF.Framework.SFConfig.FilePhysicalUrl + docEntity.FileUrl);

                    if (isSucess)
                        isSucess = service.Delete(projectId, id);
                }
                catch (Exception ex)
                {
                    new log4netProvider().Log(ex);
                    return false;
                }
#if !DEBUG
                scope.Complete();
#endif
                return isSucess;
            }
        }


        public List<DocManagementEntity> GetListAllChildByParentId(int parentId, List<DocManagementEntity> list)
        {
            List<DocManagementEntity> resultList = new List<DocManagementEntity>();
            List<DocManagementEntity> listChild = new List<DocManagementEntity>();
            listChild = list.Where(e => e.ParentID == parentId).ToList();
            int i = 0;
            foreach (var item in listChild)
            {
                i++;
                resultList.Add(item);
                resultList.AddRange(GetListAllChildByParentId(item.ID, list));
            }
            return resultList;
        }

        public bool Modify(DocManagementEntity entity)
        {
            return service.Update(entity);
        }

        /// <summary>
        /// Rename file name
        /// </summary>
        /// <param name="id">修改的文件ID</param>
        /// <param name="displayName">修改文件名字</param>
        /// <returns>修改是否成功</returns>
        public bool Modify(int id, string displayName)
        {
            DocManagementEntity entity = Get(id);
            entity.DisplayFileName = displayName;
            entity.UpdatedOn = DateTime.Now;
            return Modify(entity);
        }

        /// <summary>
        /// 修改文件目录
        /// </summary>
        /// <param name="id">修改节点ID</param>
        /// <param name="movetoId">移动到指定节点ID</param>
        /// <returns>是否成功</returns>
        public bool Modify(int projectId, int id, int movetoId)
        {
            DocManagementEntity entity = Get(id);
            List<DocManagementEntity> list = GetListAllChildByParentId(id, service.GetList(entity.ProjectID, ""));
            foreach (var item in list)
            {
                item.ProjectID = projectId;
                item.UpdatedOn = DateTime.Now;
                Modify(item);
            }
            entity.ProjectID = projectId;
            entity.ParentID = movetoId;
            entity.UpdatedOn = DateTime.Now;
            return Modify(entity);
        }

        public DocManagementEntity Get(int id)
        {
            return service.GetEntity(id);
        }

        /// <summary>
        /// 根据 parentid 查询列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<DocManagementEntity> GetList(int parentId)
        {
            return service.GetList(parentId);
        }

        /// <summary>
        /// 根据 parentid 查询列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<DocManagementEntity> GetList(int parentId, string order = "", string sort = "")
        {
            return service.GetList(parentId, order, sort);
        }

        /// <summary>
        /// 根据 parentid 查询列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<DocManagementEntity> GetList(int projectId, int parentId, string order = "", string sort = "")
        {
            return service.GetList(projectId, parentId, order, sort);
        }

        public List<DocManagementEntity> GetList(string projectId, int parentid, int userId, string order = "", string sort = "")
        {
            return service.GetList(projectId, parentid, userId, order, sort);
        }

        /// <summary>
        /// 根据用户id 查询Doc 列表，在这里会恒查出 My Document这个目录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<DocManagementEntity> GetList(string filename)
        {
            return service.GetList(filename);
        }

        public List<DocManagementEntity> GetList(string projectId, int userId, string filename)
        {
            return service.GetList(projectId, userId, filename);
        }

        /// <summary>
        /// 根据用户id 查询Doc 列表，在这里会恒查出 My Document这个目录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<DocManagementEntity> GetList(int projectId, string filename)
        {
            return service.GetList(projectId, filename);
        }

        public List<DocManagementEntity> GetListByParentID(int userId, int parentId)
        {
            return service.GetListByParentID(userId, parentId);
        }

        public List<DocManagementEntity> GetListByParentID(int userId, int parentId, int projectId, string filename)
        {
            return service.GetListByParentID(userId, parentId, projectId, filename);
        }

        /// <summary>
        /// 递归 生成tree json 
        /// </summary>
        /// <param name="parentId">父节点ID</param>
        /// <param name="list"></param>
        /// <param name="type">文件 or 文件夹</param>
        /// <returns></returns>
        public string GetInitTreeJson(int parentId, List<DocManagementEntity> list, Entity.DocManagements.DocType type)
        {
            string json = "";
            List<DocManagementEntity> listChild = new List<DocManagementEntity>();
            if (type == Entity.DocManagements.DocType.Folder)
                listChild = list.Where(e => e.ParentID == parentId && e.Type == Entity.DocManagements.DocType.Folder).ToList();
            else
                listChild = list.Where(e => e.ParentID == parentId).ToList();
            int i = 0;
            foreach (var item in listChild)
            {
                i++;
                json += "{\"id\":" + item.ID + ",\"text\":\"" + item.DisplayFileName + "\"" + ",\"projectid\":\"" + item.ProjectID + "\"";
                json += ",\"parentid\":" + item.ParentID + ",\"type\":" + (int)item.Type + ",\"userid\":" + item.UserID + ",\"children\":[";
                json += GetInitTreeJson(item.ID, list, type);
                json += "]}";
                if (i != listChild.Count)
                    json += ",";
            }
            return json;
        }

        /// <summary>
        /// 下载 文件夹 or 文件
        /// </summary>
        /// <param name="dirId">文件夹或文件的ID</param>
        /// <param name="fileurl">文件存储目录（如：1/document/），对应DocManagements中的FileUrl字段</param>
        /// <param name="tempFolderName">压缩文件的名称</param>
        public bool Download(int projectId, int dirId, string fileurl, string tempFolderName = "My Document")
        {
            DocManagementEntity entity = Get(dirId);
            if (entity.Type == Entity.DocManagements.DocType.Folder || entity.ID == 0) //下载压缩文件夹
            {
                List<DocManagementEntity> list = GetList(projectId, dirId);
                List<string> entrylist = new List<string>();
                List<DocManagementEntity> files = GetFilePaths(list, out entrylist);
                string zipName = (entity.DisplayFileName == "" ? tempFolderName : entity.DisplayFileName) + ".zip";
                return CSharpCodeStringZipper.CreateZip(entrylist, from b in files select SF.Framework.SFConfig.FilePhysicalUrl + b.FileUrl, from c in files select c.DisplayFileName, zipName, SF.Framework.SFConfig.FilePhysicalUrl + fileurl);
            }
            else if (entity.Type == Entity.DocManagements.DocType.File)     //下载文件
            {

                //1、添加编码规则Response.HeaderEncoding Response.ContentEncoding 为 utf-8
                HttpContext.Current.Response.HeaderEncoding = Encoding.UTF8;
                HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;

                HttpContext.Current.Response.ContentType = entity.FileContentType;
                //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + NoHTML(entity.DisplayFileName));（老的方法）
                //2、头部分 Content-Disposition 的设置要按照 rfc231 要求,  应该按照如下格式设置: "Content-Disposition","attachment;filename*=utf-8'zh_cn'文件名.xx"
                //   关键是 filename的设置，*= 后面是 两个单引号，分成三部分（编码 语言 文件名） 如：*=utf-8'zh_cn'文件名.xx 或者 *=utf-8' '文件名.xx
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename*=utf-8''{0}", HttpUtility.UrlPathEncode(NoHTML(entity.DisplayFileName))));
                string filename = SF.Framework.SFConfig.FilePhysicalUrl + entity.FileUrl;
                HttpContext.Current.Response.TransmitFile(filename);
            }
            return true;
        }

        public bool DownloadFiles(int projectId, string strId, string fileurl, string tempFolderName = "My Document")
        {
            string[] arrId = strId.Split('_');
            List<DocManagementEntity> list = new List<DocManagementEntity>();

            for (int i = 0; i < arrId.Length; i++)
            {
                DocManagementEntity entity = Get(Convert.ToInt32(arrId[i]));
                
                if (entity.Type == Entity.DocManagements.DocType.Folder) //下载压缩文件夹
                {
                    list.AddRange(GetList(projectId, Convert.ToInt32(arrId[i])));
                }
                else if (entity.Type == Entity.DocManagements.DocType.File)     //下载文件
                {
                    list.Add(entity);
                }
            }   
            
            List<string> entrylist = new List<string>();
            List<DocManagementEntity> files = GetFilePaths(list, out entrylist);

            string zipName = tempFolderName ?? "My Document" + ".zip";
            return CSharpCodeStringZipper.CreateZip(entrylist, from b in files select SF.Framework.SFConfig.FilePhysicalUrl + b.FileUrl, from c in files select c.DisplayFileName, zipName, SF.Framework.SFConfig.FilePhysicalUrl + fileurl);
        }

        public string NoHTML(string Htmlstring)
        {
            if (Htmlstring == null)
            {
                return "";
            }
            Htmlstring = Regex.Replace(Htmlstring, "<script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "</script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(iexcl|#161);", "\x00a1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(cent|#162);", "\x00a2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(pound|#163);", "\x00a3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, "&(copy|#169);", "\x00a9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }

        /// <summary>
        /// 获取文件路径集合
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entry">目录集合</param>
        /// <returns></returns>
        public List<DocManagementEntity> GetFilePaths(List<DocManagementEntity> list, out List<string> entry)
        {
            List<DocManagementEntity> listfile = new List<DocManagementEntity>();
            List<string> listentry = new List<string>();
            foreach (var item in list)
            {
                if (item.Type == Entity.DocManagements.DocType.Folder)
                {
                    List<DocManagementEntity> listChild = GetList(item.ProjectID, item.ID);
                    listfile.AddRange(GetFilePaths(listChild, out entry));
                    listentry.AddRange(entry);
                }
                if (!string.IsNullOrEmpty(item.FileUrl))
                {
                    listfile.Add(item);
                    listentry.Add(GetEntry(item.ID));
                }
            }

            entry = listentry;
            return listfile;
        }

        /// <summary>
        /// 获取目录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetEntry(int id)
        {
            string entry = "";
            DocManagementEntity entity = Get(id);
            if (entity.ParentID != 0)
            {
                entry += GetEntry(entity.ParentID) + "//";
            }
            if (entity.Type == Entity.DocManagements.DocType.Folder)
                entry += entity.DisplayFileName;
            return entry;
        }

        /// <summary>
        /// 获取等级目录
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DocManagementEntity> LevelDir(List<DocManagementEntity> list, int id)
        {
            List<DocManagementEntity> storeList = new List<DocManagementEntity>();
            List<DocManagementEntity> temp = list.Where(e => e.ID == id).ToList();
            if (temp.Count > 0)
            {
                storeList.AddRange(temp);
                storeList.AddRange(LevelDir(list, temp[0].ParentID));
            }
            return storeList;
        }
        /// <summary>
        /// project id为0的所有文件
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<DocManagementEntity> GetListByUserId(int userId)
        {
            return service.GetListByUserId(userId);
        }
    }
}
