using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using System.IO;
using SF.Framework.EmailSender;
using SF.Framework.EmailSender.Providers;
using SF.Framework.File;
using SF.Framework.Log.Providers;
using SF.Framework.StringZipper.Providers;
using FamilyBook.Entity.DocManagements;
using FamilyBook.Business.DocManagement;
using FamilyBook.Business;
using Newtonsoft.Json.Converters;
using FamilyBook.Web.Controllers;
using SF.Framework.Utils;
using FamilyBook.Entity;
using System.Xml.Serialization;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework;
using System.IO;
using WebGrease.Css;
using MailPriority = System.Net.Mail.MailPriority;

namespace PM.Document.Web.Areas.DocManagement.Controllers
{

    public class DocHomeController : BaseController
    {
        //
        // GET: /DocManagement/Home/

        private bool ISIE
        {
            get
            {
                return Request.Browser.Browser.Equals("ie", StringComparison.CurrentCultureIgnoreCase)
                    || Request.Browser.Browser.Equals("InternetExplorer", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        protected string IEEmulator
        {
            get
            {
                if (!ISIE)
                    return "";
                var version = 8.0f;
                if (float.TryParse(Request.Browser.Version, out version) && version < 8)
                {
                    //var ies =
                    //    Request.Browser.Browsers.ToArray()
                    //        .Select(x => x.ToString())
                    //        .ToList()
                    //        .Find(x => x.Contains("to"));
                    //if (ies != null)
                    //    float.TryParse(ies.Substring(ies.IndexOf("to") + 2), out version);
                    version = 8;
                }
                return string.Format("<meta http-equiv='X-UA-Compatible' content='IE=EmulateIE{0}' />", (int)version);
            }
        }

        DocManagementBusiness business = new DocManagementBusiness();

        private List<string> folderList=new List<string>(); 

        public ActionResult Index(int projectId = 0, int folderId = 0)
        {
            ViewBag.UserID = UserID;
            ViewBag.UserType = UserType;
            ViewBag.ProjectID = projectId;
            ViewBag.FolerId = folderId;
            Session["DocOrderField"] = "DisplayFileName";
            Session["DocSort"] = "ASC";
            ViewBag.InitDataJson = GetInitJson();
            ViewBag.IEEmulator = IEEmulator;
            return View();
        }

        public ActionResult Move(int id, int projectId)
        {
            //List<DocManagementEntity> list = business.GetList(projectId, "");
            //List<DocManagementEntity> selfList = business.GetListAllChildByParentId(id, list);
            //foreach (var item in selfList)
            //{
            //    list.Remove(item);
            //}
            //list.RemoveAll(e => e.ID == id);
            //string strJson = "[" + business.GetInitTreeJson(0, list, FamilyBook.Entity.DocManagements.DocType.Folder) + "]";
            string strJson = GetInitJson(DocType.Folder, "move");
            ViewBag.DefaultData = strJson;
            ViewBag.MoveID = id;
            var filename = business.Get(id).DisplayFileName;
            ViewBag.FileName = filename.Length > 25 ? filename.Substring(0, 10) + "..." + filename.Substring(filename.Length - 10) : filename;
            return View();
        }

        public ActionResult NewFolder(int id)
        {
            ViewBag.ID = id;
            return View();
        }

        public ActionResult Rename(int id)
        {
            ViewData["FileName"] = business.Get(id).DisplayFileName;
            ViewBag.ID = id;
            return View();
        }

        public ActionResult UploadDoc(int projectId, int id, string dir)
        {
            ViewBag.ID = id;
            ViewBag.Dir = dir;
            ViewBag.ProjectId = projectId;
            ViewBag.UserID = UserID;
            return View();
        }

        public string GetInitJson(DocType docType = DocType.File, string status = "index")
        {
            IList<ProjectDetailDTO> listPro = GetProject(UserID);
            string str = "[";
            str += "{\"id\":" + 0;
            str += ",\"text\":" + "\"My Document\"";
            str += ",\"projectid\":" + 0;
            str += ",\"parentid\":" + 0;
            str += ",\"type\":" + 1;
            str += ",\"userid\":" + 0;
            str += ",\"children\":[";
            for (int i = 0; i < listPro.Count; i++)
            {
                var item = listPro[i];
                int proId = item.ProjectID;
                str += "{\"id\":" + 0;
                str += ",\"text\":" + "\"" + item.Title + "\"";
                str += ",\"projectid\":" + item.ProjectID;
                str += ",\"parentid\":" + 0;
                str += ",\"type\":" + 1;
                str += ",\"userid\":" + UserID;

                List<DocManagementEntity> list = business.GetList(proId, "");

                str += ",\"children\":[" + business.GetInitTreeJson(0, list, docType) + "]";
                str += "}";

                if (listPro.Count > i + 1)
                {
                    str += ",";
                }
            }
            List<DocManagementEntity> listSelf = business.GetListByUserId(UserID);
            if (status == "move" && listSelf.Count > 0)
            {
                listSelf = listSelf.Where(o => o.Type == DocType.Folder).ToList();
            }
            if (listPro.Count > 0 && listSelf.Count > 0)
                str += ",";

            str += business.GetInitTreeJson(0, listSelf, DocType.File);

            str += "]}]";
            return str;
        }

        public bool Delete(int projectId, int id)
        {
            return business.Delete(projectId, id);
        }

        public bool Modify(int id, string text)
        {
            return business.Modify(id, text);
        }

        public bool MoveTo(int projectid, int movetoid, int moveid)
        {

            return business.Modify(projectid, moveid, movetoid);
        }

        public void Download(int projectId, int id, string tempFolderName)
        {
            if (tempFolderName == "")
                business.Download(projectId, id, "/document/");
            else
                business.Download(projectId, id, "/document/", tempFolderName);
        }
        public void DownloadFiles(int projectId, string strId, string tempFolderName)
        {
            if (tempFolderName == "")
                business.DownloadFiles(projectId, strId, "/document/");
            else
                business.DownloadFiles(projectId, strId, "/document/", tempFolderName);
        }

        public string GetListByParentID(int parentid, int projectId, string order = "", string sort = "")
        {
            System.Web.HttpContext.Current.Session["DocOrderField"] = order;
            System.Web.HttpContext.Current.Session["DocSort"] = sort;
            IsoDateTimeConverter JSONDateFormat = new IsoDateTimeConverter() { DateTimeFormat = "MM/dd/yyyy HH:mm" };
            List<DocManagementEntity> list = new List<DocManagementEntity>();
            if (projectId == 0)
            {
                var strProIds = GetProjectIds();
                list = business.GetList(strProIds, parentid, UserID, order, sort);
            }
            else
                list = business.GetList(projectId, parentid, order, sort);
            return Newtonsoft.Json.JsonConvert.SerializeObject(list, JSONDateFormat);
        }

        private string GetProjectIds()
        {
            IList<ProjectDetailDTO> listPro = GetProject(UserID);
            var strProIds = "";
            for (int i = 0; i < listPro.Count; i++)
            {
                if (strProIds != "")
                    strProIds += ",";
                int proId = 0;
                int.TryParse(listPro[i].ProjectID.ToString(), out proId);
                strProIds += proId;
            }
            return strProIds;
        }

        public string GetOrderByInfo()
        {
            return "[{\"Order\":\"" + System.Web.HttpContext.Current.Session["DocOrderField"] + "\",\"Sort\":\"" + System.Web.HttpContext.Current.Session["DocSort"] + "\"}]";
        }

        public string SearchList(string filename)
        {
            IsoDateTimeConverter JSONDateFormat = new IsoDateTimeConverter() { DateTimeFormat = "MM/dd/yyyy HH:mm" };
            IList<ProjectDetailDTO> listPro = GetProject(UserID);
            var strProIds = "";
            for (int i = 0; i < listPro.Count; i++)
            {
                if (strProIds != "")
                    strProIds += ",";
                int proId = 0;
                int.TryParse(listPro[i].ProjectID.ToString(), out proId);
                strProIds += proId;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(business.GetList(strProIds, UserID, filename), JSONDateFormat);
        }

        public string GetSiteMap(int id)
        {
            List<DocManagementEntity> list = business.GetList(UserID, "");
            List<DocManagementEntity> storeList = business.LevelDir(list, id);

            return Newtonsoft.Json.JsonConvert.SerializeObject(storeList);
        }

        public bool IsDuplicationReName(int id, int projectId, string filename)
        {
            DocManagementEntity entity = business.Get(id);
            return business.GetListByParentID(UserID, entity.ParentID, projectId, filename).Where(e => e.ID != id).ToList().Count > 0;
        }

        public bool IsDuplicationName(int parentId, int projectId, string filename)
        {
            return business.GetListByParentID(UserID, parentId, projectId, filename).Count > 0;
        }

        public int AddFolder(int parentId, int projectId, string folderName)
        {
            DocManagementEntity entity = new DocManagementEntity();
            entity.ProjectID = projectId;
            entity.UserID = UserID;
            entity.Type = FamilyBook.Entity.DocManagements.DocType.Folder;
            entity.ParentID = parentId;
            entity.FileName = folderName;
            entity.DisplayFileName = folderName;

            return business.Insert(entity);
        }

        #region define a delegate for sendemail 
        private delegate void Del_SendEmail(int a, int b, string s1, string s2);
        #endregion

        public bool SaveFile(int projectId, string jsondata, int parentId)
        {
            List<SF.Framework.File.FileEntity> list = new List<SF.Framework.File.FileEntity>();
            if (!string.IsNullOrEmpty(jsondata))
            {
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SF.Framework.File.FileEntity>>(jsondata);
            }
            bool b = business.InsertList(projectId, list, parentId, UserID, CompanyID);


            if (b == true)
            {
                string path = GetPath(projectId, parentId);
                string fileDescription = list.Count == 1 ? "a file" : list.Count + " files";
                string filePath = string.Empty;
                foreach (var item in list)
                {
                    filePath += path + item.DisplayName + "\r\n";
                }
                Del_SendEmail handler = new Del_SendEmail(SendEmailForUploading);
                handler.BeginInvoke(projectId, UserID, fileDescription, filePath, null, null);
            }
            return b;
        }

        #region Get Path
        private string GetPath(int projectId, int parentId)
        {
            string projectTitle = "";
            ProjectApplication projectApp = new ProjectApplication();
            ProjectsEntity projectEntity = projectApp.Get(projectId);
            if (projectEntity != null)
                projectTitle = projectEntity.Title;
            else
                projectTitle = "My Document";
            string folderPath = string.Empty;
            List<DocManagementEntity> docList = business.GetList(projectId, "");
            if (parentId != 0)
            {
                GetFolderName(docList, parentId);
            }
            for (int i = folderList.Count-1; i >= 0; i--)
            {
                folderPath += folderList[i];
            }
            return "> " + projectTitle + @"\" + folderPath;
        }
        #endregion

        #region Get Folder Name
        //private void GetFolderName(ref string folderPath, List<DocManagementEntity> docManagement, int parentId)
        //{
        //    var doc = docManagement.Find(o => o.ID == parentId);
        //    folderPath += doc.FileName + @"\";
        //    if (doc.ParentID != 0)
        //    {
        //        GetFolderName(ref folderPath, docManagement, doc.ParentID);
        //    }
        //}
        private void GetFolderName(List<DocManagementEntity> docManagement, int parentId)
        {
            var doc = docManagement.Find(o => o.ID == parentId);
            //folderPath += doc.FileName + @"\";
            folderList.Add(doc.FileName + @"\");
            if (doc.ParentID != 0)
            {
                GetFolderName(docManagement, doc.ParentID);
            }
        }
        #endregion

        #region Get Email Template
        private string GetEmailTemplate(string filename, string folder = "")
        {
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Template\\" + filename;
            return System.IO.File.Exists(filePath) ? System.IO.File.ReadAllText(filePath) : "";
        }
        #endregion

        #region Send email Afer Clinet or PM Uploaded
        private void SendEmailForUploading(int projectId, int userId, string fileDescription, string filePath)
        {
            if (projectId == 0)
            {
                return;
            }
            UserApplication userApp = new UserApplication();
            ProjectApplication projectApp = new ProjectApplication();
            UsersEntity usersEntity = userApp.GetUser(userId);

            //发邮件的角色必须是Client或PM
            if (usersEntity.Role != RolesEnum.CLIENT && usersEntity.Role != RolesEnum.PM && usersEntity.Status == Status.Active.ToString().ToUpper())
            {
                return;
            }

            ProjectsEntity projectEntity = projectApp.Get(projectId);
            IEmailSender smtpClient = new SmtpClientEmailSender(new TextFileLogger());

            //邮件参数初始化
            string contentTemplete = GetEmailTemplate("SendEmailToCilentAndPMWhenUploading.txt").Trim();
            if (!string.IsNullOrEmpty(contentTemplete))
            {
                contentTemplete = contentTemplete.Replace("[UploadUser]", usersEntity.FirstName)
                                                 .Replace("[ProjectTitle]", projectEntity.Title)
                                                 .Replace("[DateTime]", DateTime.Now.ToString("MM/dd/yyyy HH:mm"))
                                                 .Replace("[FileDescription]", fileDescription)
                                                 .Replace("[FilePath]", filePath);
            }
            string to = string.Empty;
            string subject = string.Format("Notice - {0} uploaded a file or some files ", usersEntity.FirstName);

            List<int> userIdList = projectApp.GetActiveUserIdByProjectId(projectId);
            if (userIdList != null && userIdList.Count > 0)
            {
                userIdList.RemoveAll(o=>o==userId);
                foreach (int item in userIdList)
                {
                    var userItem = userApp.GetUser(item);
                    if (userItem.Role == RolesEnum.CLIENT || userItem.Role == RolesEnum.PM)
                    {
                        to = userItem.Email;
                        if (!string.IsNullOrEmpty(contentTemplete))
                        {
                            string content = contentTemplete.Replace("[FirstName]", userItem.FirstName)
                                                             .Replace("[LastName]", userItem.LastName);
                            smtpClient.SendMail(to, subject, content, false, MailPriority.Normal);
                        }
                    }
                }
            }
        }
        #endregion

        //pm dashboard data source
        public string GetRemoteProjectFile()
        {
            string projectId = GetProjectIds();
            IsoDateTimeConverter JSONDateFormat = new IsoDateTimeConverter() { DateTimeFormat = "MM/dd/yyyy HH:mm" };
            List<DocManagementEntity> list = business.GetList(projectId, 0, UserID);
            return Newtonsoft.Json.JsonConvert.SerializeObject(list, JSONDateFormat);
        }

        //public string GetClientTopMenu()
        //{
        //    ProjectApi.ProjectApiSoapClient client = new ProjectApi.ProjectApiSoapClient();
        //    return client.GetClientTopMenu(UserID);
        //}

        public IList<ProjectDetailDTO> GetProject(int userId)
        {
            ProjectApplication projApp = new ProjectApplication();
            UsersEntity user = new UserApplication().GetUser(userId);
            IOrderedEnumerable<ProjectDetailDTO> list = projApp.GetUserProjects(user).OrderBy(r => r.Title);
            return list.ToList<ProjectDetailDTO>();
        }
    }
}
