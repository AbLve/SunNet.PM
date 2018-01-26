using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace SunNet.PMNew.Web.Sunnet.Profile
{
    /// <summary>
    /// Summary description for DoSealRequestEdit
    /// </summary>
    public class DoSealRequestEdit : DoBase, IHttpHandler
    {
        SealsApplication app = new SealsApplication();
        string approveEmailPath = string.Empty;
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            int sealRequestID = QF(request.Form["ctl00$ContentPlaceHolder1$hdID"], 0);
            string type = request.QueryString["type"];
            string result = string.Empty;
            switch (type)
            {
                case "Sealed":
                    {
                        result = Sealed(sealRequestID);
                        break;
                    }
                case "Cancel":
                    {
                        result = Cancel(sealRequestID);
                        break;
                    }
                case "Approved":
                    {
                        result = Approved(sealRequestID, context.Server);
                        break;
                    }
                case "Denied":
                    {
                        result = Denied(sealRequestID, context.Server);
                        break;
                    }
                case "Completed":
                    {
                        result = Completed(sealRequestID);
                        break;
                    }
                case "Save":
                    {
                        string title = request.Form["ctl00$ContentPlaceHolder1$txtTitle"];
                        string description = request.Form["ctl00$ContentPlaceHolder1$txtDescription"];
                        List<string> sealKeys = GetSealKeys(request);
                        result = Save(sealRequestID, sealKeys, title, description, context.Request.Files);
                        break;
                    }
                case "Submit":
                    {
                        List<string> sealKeys = GetSealKeys(request);
                        result = Submit(sealRequestID, sealKeys, context.Server);
                        break;
                    }
                default:
                    {
                        result = "-1";
                        break;
                    }
            }
            context.Response.Write(result);
            context.Response.End();
        }

        private List<string> GetSealKeys(HttpRequest request)
        {
            string sealkey = request.Form["ctl00$ContentPlaceHolder1$hdChklistKeys"];
            string[] allkeys = request.Form.AllKeys;
            List<string> sealKeys = sealkey.Split(',').ToList<string>();
            List<string> chkKeys = new List<string>();
            foreach (string key in allkeys)
            {
                Regex regex = new Regex(@"^ctl00\$ContentPlaceHolder1\$chklistSeal\$([0-9]+)");
                if (regex.IsMatch(key))
                {
                    Match match = regex.Match(key);
                    chkKeys.Add(sealKeys[int.Parse(match.Groups[1].Value)]);
                }
            }
            return chkKeys;
        }

        protected string Save(int sealRequestID, List<string> sealKeys, string title, string description, HttpFileCollection files)
        {
            SealRequestsEntity sealRequestsEntity = new SealRequestsEntity();

            if (sealRequestID == 0) //add
            {
                List<SealsEntity> list = app.GetList().FindAll(r => r.Status == Status.Active);
                sealRequestsEntity.Title = title;
                sealRequestsEntity.Description = description.Replace("\r\n", "<br>");
                sealRequestsEntity.RequestedBy = UserID;
                sealRequestsEntity.RequestedDate = DateTime.Now;
                sealRequestsEntity.Status = RequestStatus.Open;

                sealRequestsEntity.SealList = new List<SealsEntity>();
                foreach (string sealKey in sealKeys)
                {

                    SealsEntity sealsEntity = list.Find(r => r.ID == int.Parse(sealKey));
                    if (sealsEntity != null)
                    {
                        sealRequestsEntity.SealList.Add(sealsEntity);
                    }

                }
                if (sealRequestsEntity.SealList.Count == 0)
                {
                    //ShowFailMessageToClient("Please select seal.");
                    return "4";
                }

                if ((sealRequestsEntity.ID = app.SealRequestsInsert(sealRequestsEntity)) > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        if (files[i].ContentLength > 0)
                        {
                            InsertFile(files[i], sealRequestsEntity.ID, 1, i);
                        }

                    }
                    return "1";
                }
                else
                {
                    return "2";
                }
            }
            else //edit
            {
                if ((sealRequestsEntity = CheckData(sealRequestID)) != null)
                {
                    if (sealRequestsEntity.Status < RequestStatus.Approved && sealRequestsEntity.RequestedBy == UserID)
                    {
                        if (title != null)
                        {
                            sealRequestsEntity.Title = title;
                        }
                        if (description != null)
                        {
                            sealRequestsEntity.Description = description.Replace("\r\n", "<br>");
                        }

                        List<SealsEntity> list = app.GetList().FindAll(r => r.Status == Status.Active);

                        if (sealKeys.Count != 0)
                        {
                            if (sealRequestsEntity.SealList == null)
                            {
                                sealRequestsEntity.SealList = new List<SealsEntity>();
                            }
                            foreach (string sealKey in sealKeys)
                            {
                                SealsEntity sealsEntity = list.Find(r => r.ID == int.Parse(sealKey));

                                if (sealsEntity != null)
                                {
                                    sealRequestsEntity.SealList.Add(sealsEntity);
                                }
                            }
                            if (!app.SealRequestsUpdate(sealRequestsEntity))
                            {
                                //ShowFailMessageToClient(app.BrokenRuleMessages);
                                return "2";
                            }
                        }
                        for (int i = 0; i < files.Count; i++)
                        {
                            if (files[i].ContentLength > 0)
                            {
                                InsertFile(files[i], sealRequestsEntity.ID, 1, i);
                            }
                        }
                        return "1";
                    }
                    else
                    {
                        // ShowFailMessageToClient("It is not editable.");
                        return "2";
                    }
                }
                return "2";
            }
        }

        protected string Cancel(int sealRequestID)
        {
            SealRequestsEntity sealRequestsEntity = CheckData(sealRequestID);
            if (sealRequestsEntity != null)
            {
                if (sealRequestsEntity.Status != RequestStatus.Open || sealRequestsEntity.RequestedBy != UserID)
                {
                    //return "0";
                    return "0";
                }
                if (app.UpateStatus(sealRequestsEntity.ID, RequestStatus.Cancel))
                {
                    return "1";
                }
                else
                    //return "2";
                    return "2";
            }
            return "2";
        }

        /// <summary>
        /// Submit
        /// </summary>
        protected string Submit(int sealRequestID, List<string> sealKeys, HttpServerUtility server)
        {
            SealRequestsEntity sealRequestsEntity = CheckData(sealRequestID);
            if (sealRequestsEntity != null)
            {
                if (sealRequestsEntity.Status != RequestStatus.Open || sealRequestsEntity.RequestedBy != UserID)
                {
                    return "0";
                }

                List<SealsEntity> list = app.GetList().FindAll(r => r.Status == Status.Active);
                sealRequestsEntity.SealList = new List<SealsEntity>();
                foreach (string key in sealKeys)
                {
                    SealsEntity sealsEntity = list.Find(r => r.ID == int.Parse(key));
                    if (sealsEntity != null)
                    {
                        sealRequestsEntity.SealList.Add(sealsEntity);
                    }
                }

                if (sealRequestsEntity.SealList.Count == 0)
                {
                    //ShowFailMessageToClient("Please select seal.");
                    return "4";
                }

                if (app.UpateStatus(sealRequestsEntity.ID, RequestStatus.Submit))
                {
                    string mailTemplatePath = server.MapPath(@"~\Template\SendEmailToApproved.txt");
                    string mailTemplate = File.ReadAllText(mailTemplatePath);
                    string mailTitle = "[申请公章] " + sealRequestsEntity.Title;

                    foreach (SealUnionRequestsEntity unionEntity in app.GetApprovedByList(sealRequestsEntity.ID))
                    {
                        UserApplication userApplication = new UserApplication();
                        UsersEntity user = userApplication.GetUser(UserID);
                        string content = mailTemplate.Replace("[ClientName]", unionEntity.FirstName).Replace("[applicant]", user.FirstName)
                             .Replace("[content]", sealRequestsEntity.Description);

                        if (Config.IsTest)
                        {
                            new SmtpClientEmailSender(new TextFileLogger()).SendMail(Config.TestMails, Config.DefaultSendEmail, mailTitle, content);
                        }
                        else
                        {
                            new SmtpClientEmailSender(new TextFileLogger()).SendMail(unionEntity.Email, Config.DefaultSendEmail, mailTitle, content);
                        }
                    }
                    return "1";
                }
                else
                {
                    return "2";
                }
            }
            else
            {
                return "2";
            }
        }

        /// <summary>
        /// Approved
        /// </summary>
        protected string Approved(int sealRequestID, HttpServerUtility server)
        {
            SealRequestsEntity sealRequestsEntity = CheckData(sealRequestID);
            if (sealRequestsEntity != null)
            {
                if (sealRequestsEntity.Status == RequestStatus.Submit)
                {
                    List<SealUnionRequestsEntity> list = app.GetSealUnionRequestsList(sealRequestsEntity.ID);
                    if (list.Find(r => r.ApprovedBy == UserID) != null)
                    {
                        if (app.SealRequestApproved(sealRequestsEntity.ID, true))
                        {
                            string mailTemplatePath = server.MapPath(@"~\Template\SendEmailToSealed.txt");
                            string mailTemplate = File.ReadAllText(mailTemplatePath);
                            string mailTitle = "[批准使用公章] " + sealRequestsEntity.Title;

                            UsersEntity usersEntity = new App.UserApplication().GetUser(sealRequestsEntity.RequestedBy);

                            foreach (SealUnionRequestsEntity user_unionEntity in app.GetSealedByList(sealRequestsEntity.ID))
                            {
                                string content = mailTemplate.Replace("[ClientName]", user_unionEntity.FirstName).Replace("[applicant]", usersEntity.FirstName)
                                    .Replace("[content]", sealRequestsEntity.Description);

                                if (Config.IsTest)
                                {
                                    new SmtpClientEmailSender(new TextFileLogger()).SendMail(Config.TestMails, Config.DefaultSendEmail, mailTitle, content);
                                }
                                else
                                {
                                    new SmtpClientEmailSender(new TextFileLogger()).SendMail(user_unionEntity.Email, Config.DefaultSendEmail, mailTitle, content);
                                }
                            }
                            return "1";
                        }
                        else
                            return "2";
                    }
                    else
                        return "0";

                }
            }
            return "2";
        }

        /// <summary>
        /// Denied
        /// </summary>
        protected string Denied(int sealRequestID, HttpServerUtility server)
        {
            SealRequestsEntity sealRequestsEntity = CheckData(sealRequestID);
            if (sealRequestsEntity != null)
            {
                if (sealRequestsEntity.Status == RequestStatus.Submit)
                {
                    foreach (SealUnionRequestsEntity unionEntity in app.GetSealUnionRequestsList(sealRequestsEntity.ID))
                    {
                        if (unionEntity.ApprovedBy == UserID)
                        {
                            if (app.SealRequestApproved(sealRequestsEntity.ID, false))
                            {

                                string mailTemplatePath = server.MapPath(@"~\Template\SendEmailSealDenied.txt");
                                string mailTemplate = File.ReadAllText(mailTemplatePath);
                                string mailTitle = "[申请失败] " + sealRequestsEntity.Title;

                                UsersEntity usersEntity = new App.UserApplication().GetUser(sealRequestsEntity.RequestedBy);

                                string content = mailTemplate.Replace("[ClientName]", usersEntity.FirstName)
                                        .Replace("[content]", sealRequestsEntity.Description);

                                if (Config.IsTest)
                                {
                                    new SmtpClientEmailSender(new TextFileLogger()).SendMail(Config.TestMails, Config.DefaultSendEmail, mailTitle, content);
                                }
                                else
                                {
                                    new SmtpClientEmailSender(new TextFileLogger()).SendMail(usersEntity.Email, Config.DefaultSendEmail, mailTitle, content);
                                }

                                return "1";
                            }
                            else
                                return "2";
                        }
                    }
                }
                else
                {
                    return "0";
                }
            }
            return "2";
        }

        /// <summary>
        /// Seal
        /// </summary>
        protected string Sealed(int sealRequestID)
        {
            SealRequestsEntity sealRequestsEntity = CheckData(sealRequestID);
            if (sealRequestsEntity != null)
            {
                if (sealRequestsEntity.Status == RequestStatus.Approved)
                {
                    foreach (SealUnionRequestsEntity unionEntity in app.GetSealUnionRequestsList(sealRequestsEntity.ID))
                    {
                        if (unionEntity.SealedBy == UserID)
                        {
                            if (app.SealRequestSealed(sealRequestsEntity.ID, UserID))
                            {
                                return "1";
                            }
                            else
                            {
                                return "2";
                            }
                        }
                    }
                }
                else
                {
                    return "0";
                }
            }
            return "2";
        }

        protected string Completed(int sealRequestID)
        {
            SealRequestsEntity sealRequestsEntity = CheckData(sealRequestID);
            if (sealRequestsEntity != null)
            {
                if (sealRequestsEntity.Status == RequestStatus.Sealed && sealRequestsEntity.RequestedBy == UserID)
                {
                    if (app.UpateStatus(sealRequestsEntity.ID, RequestStatus.Complete))
                    {
                        return "1";
                    }
                    else
                        return "2";
                }
                else
                {
                    return "0";
                }
            }
            return "2";
        }

        private SealRequestsEntity CheckData(int sealRequestID)
        {
            if (sealRequestID > 0)
            {
                SealRequestsEntity sealRequestsEntity = app.GetSealRequests(sealRequestID);
                if (sealRequestsEntity == null)
                {
                    return null;
                }
                return sealRequestsEntity;
            }
            else
            {
                return null;
            }
        }

        private void InsertFile(HttpPostedFile file, int sealRequestId, int type, int index)
        {
            string fileName = file.FileName;
            string tmpFileName = string.Format("{0}{2}{1}", DateTime.Now.ToString("MMddyyHHmmss"), fileName.Substring(fileName.LastIndexOf(".")), index);
            Console.WriteLine(tmpFileName);
            file.SaveAs(Config.SealFilePath + tmpFileName);
            SealFileEntity fileEntity = new SealFileEntity();
            fileEntity.Title = string.Empty;
            fileEntity.Name = fileName;
            fileEntity.Path = Config.SealFilePath + tmpFileName;
            fileEntity.SealRequestsID = sealRequestId;
            fileEntity.UserID = UserID;
            fileEntity.Type = type;
            fileEntity.IsDeleted = false;
            fileEntity.CreateOn = DateTime.Now;
            app.SealFilesInsert(fileEntity);
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