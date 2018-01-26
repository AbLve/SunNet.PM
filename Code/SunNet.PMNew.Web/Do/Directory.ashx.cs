using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Core;
using StructureMap;
using SunNet.PMNew.Web.Codes;
using System.Text;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Directory : IHttpHandler
    {
        private DirectoryEntity GetDirectoryFromRequest(HttpContext context)
        {
            try
            {
                DirectoryEntity model = FileFactory.CreateDirectoryEntity(IdentityContext.UserID, ObjectFactory.GetInstance<ISystemDateTime>());

                model.ID = int.Parse(context.Request.Params["id"]);
                model.Title = context.Request.Params["title"];
                model.Description = context.Request.Params["description"];
                model.Logo = " ";
                return model;
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                return null;
            }
        }
        private DirectoryObjectsEntity GetObjectFromRequest(HttpContext context)
        {
            try
            {
                DirectoryObjectsEntity model = FileFactory.CreateDirectoryObject(IdentityContext.UserID, ObjectFactory.GetInstance<ISystemDateTime>());
                model.DirectoryID = int.Parse(context.Request["directoryid"]);
                model.ObjectID = int.Parse(context.Request["objectid"]);
                model.Type = context.Request.Params["objecttype"];
                model.Logo = model.Type + ".png";

                return model;
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                return null;
            }
        }

        private string GetResponse(bool success, string msg, int value)
        {
            ResponseMessage m = new ResponseMessage();
            m.Success = success;
            m.MessageContent = msg;
            m.Value = value.ToString();

            return UtilFactory.Helpers.JSONHelper.GetJson<ResponseMessage>(m);
        }
        private string SuccessMessage = "Operation successful.";
        private string FailMessage = "Operation failed.";
        private bool UpdateDirectory(HttpContext context, out string msg, out int id)
        {
            DirectoryEntity model = GetDirectoryFromRequest(context);
            FileApplication fileApp = new FileApplication();
            id = fileApp.UpdateDirectory(model);
            msg = string.Empty;
            if (fileApp.BrokenRuleMessages.Count > 0)
            {
                msg = string.Format("{0},{1}", fileApp.BrokenRuleMessages[0].Key, fileApp.BrokenRuleMessages[0].Message);
                return false;
            }
            return true;
        }
        private bool InsertObjects(HttpContext context, out string msg, out int id)
        {
            DirectoryObjectsEntity model = GetObjectFromRequest(context);
            FileApplication fileApp = new FileApplication();
            msg = string.Empty;
            id = fileApp.PushObjectToDirectory(model);
            if (fileApp.BrokenRuleMessages.Count > 0)
            {
                msg = string.Format("{0},{1}", fileApp.BrokenRuleMessages[0].Key, fileApp.BrokenRuleMessages[0].Message);
                return false;
            }
            return true;
        }
        private string GetObjects(int directoryID)
        {
            FileApplication fileApp = new FileApplication();
            List<DirectoryEntity> list = fileApp.GetDirectories(directoryID);
            if (list == null && list.Count == 0)
            {
                return "[]";
            }
            string jsons = UtilFactory.Helpers.JSONHelper.GetJson<List<DirectoryEntity>>(list);
            return jsons;
        }
        private Dictionary<string, int> GetDeleteList(string paras)
        {
            try
            {
                if (string.IsNullOrEmpty(paras))
                    return null;
                Dictionary<string, int> list = new Dictionary<string, int>();
                string[] items = paras.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in items)
                {
                    string[] keyorvalue = item.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    list.Add(keyorvalue[0], int.Parse(keyorvalue[1]));
                }
                return list;
            }
            catch
            {
                return null;
            }

        }
        private string DeleteObjects(HttpContext context)
        {
            string msg = string.Empty;
            string paras = context.Request["objects"];
            if (string.IsNullOrEmpty(paras))
            {
                return "Arguments Error!";
            }
            FileApplication fileApp = new FileApplication();
            string[] items = paras.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (items.Length > 0)
            {
                foreach (string item in items)
                {
                    string[] keyorvalue = item.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (keyorvalue.Length == 3)
                    {
                        int id = 0;
                        int fileid = 0;
                        if (int.TryParse(keyorvalue[1], out id)
                            && int.TryParse(keyorvalue[2], out fileid)
                            && keyorvalue[0] == DirectoryObjectType.Directory.ToString())
                        {
                            if ((!fileApp.RemoveDirectory(id)) && fileApp.BrokenRuleMessages.Count > 0)
                            {
                                msg += fileApp.BrokenRuleMessages[0].Message;
                            }
                        }
                        else
                        {
                            if (!fileApp.RemoveObjects(id, fileid) && fileApp.BrokenRuleMessages.Count > 0)
                            {
                                msg += fileApp.BrokenRuleMessages[0].Message;
                            }
                        }
                    }
                }
                return msg;
            }
            else
            {
                return "Arguments Error!";
            }
        }
        private string MoveObjects(HttpContext context)
        {
            string msg = string.Empty;
            string paras = context.Request["objects"];
            int target = 0;
            if (int.TryParse(context.Request["target"], out target))
            {
                if (string.IsNullOrEmpty(paras))
                {
                    return "Arguments Error!";
                }
                FileApplication fileApp = new FileApplication();
                string[] items = paras.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (items.Length > 0)
                {
                    StringBuilder sbdireids = new StringBuilder("0,");
                    StringBuilder sbobjids = new StringBuilder("0,");
                    foreach (string item in items)
                    {
                        string[] keyorvalue = item.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (keyorvalue.Length == 2)
                        {
                            int id = 0;
                            if (int.TryParse(keyorvalue[1], out id) && keyorvalue[0] == DirectoryObjectType.Directory.ToString())
                            {
                                sbdireids.Append(id);
                                sbdireids.Append(",");
                            }
                            else
                            {
                                sbobjids.Append(id);
                                sbobjids.Append(",");
                            }
                        }
                    }
                    sbdireids.Append(";");
                    sbdireids.Append(sbobjids);
                    if (!fileApp.ChangeParent(sbdireids.ToString(), target) && fileApp.BrokenRuleMessages.Count > 0)
                    {
                        msg = fileApp.BrokenRuleMessages[0].Message;
                    }
                    return msg;
                }
                else
                {
                    return "Arguments Error!";
                }
            }
            else
            {
                return "Arguments Error!";
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (IdentityContext.UserID <= 0)
            {
                context.Response.Write("[]");
                return;
            }
            if (string.IsNullOrEmpty(context.Request.Params["type"]))
            {
                context.Response.Write("");
                return;
            }
            string type = context.Request.Params["type"];
            string response = string.Empty;
            string msg = string.Empty;
            int id = 0;
            switch (type)
            {
                case "UpdateDirectory":
                    if (UpdateDirectory(context, out msg, out id))
                    {
                        response = GetResponse(true, SuccessMessage, id);
                    }
                    else
                    {
                        response = GetResponse(false, msg, id);
                    }
                    context.Response.Write(response);
                    break;
                case "UpdateObject":
                    if (InsertObjects(context, out msg, out id))
                    {
                        response = GetResponse(true, SuccessMessage, id);
                    }
                    else
                    {
                        response = GetResponse(false, msg, id);
                    }
                    context.Response.Write(response);
                    break;
                case "DeleteObjects":
                    msg = DeleteObjects(context);
                    if (string.IsNullOrEmpty(msg))
                    {
                        response = GetResponse(true, SuccessMessage, id);
                    }
                    else
                    {
                        response = GetResponse(false, msg, id);
                    }

                    context.Response.Write(response);
                    break;
                case "GetObjects":
                    int direcotyID = -1;
                    if (int.TryParse(context.Request.Params["parentID"], out direcotyID))
                    {
                        string result = GetObjects(direcotyID);
                        context.Response.Write(result);
                    }
                    else
                    {
                        context.Response.Write("[]");
                    }
                    context.Response.Write(response);
                    break;
                case "MoveObjects":
                    msg = MoveObjects(context);
                    if (string.IsNullOrEmpty(msg))
                    {
                        response = GetResponse(true, SuccessMessage, id);
                    }
                    else
                    {
                        response = GetResponse(false, msg, id);
                    }
                    context.Response.Write(response);
                    break;
                default: break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
