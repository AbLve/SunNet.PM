using System.Linq;
using System.Transactions;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Web;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProjectModel;
using System.IO;
using NPOI.HSSF;
using SunNet.PMNew.Framework.Utils.Providers;
using FileHelper = SunNet.PMNew.PM2014.Codes.FileHelper;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.PM2014.UserControls.Ticket;
using SunNet.PMNew.Framework.Extensions;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for DoDeleteOwnFeedBack
    /// </summary>
    public class DoDeleteOwnFeedBack : DoBase, IHttpHandler
    {
        FeedBackApplication fbAPP = new FeedBackApplication();
        FileApplication fileApp = new FileApplication();
        ProjectApplication projectApp = new ProjectApplication();
        TicketsApplication ticketApp = new TicketsApplication();
        TicketStatusManagerApplication ticketStatusMgr = new TicketStatusManagerApplication();
        FeedbackList feedback = new FeedbackList();
        TicketsApplication ticketAPP = new TicketsApplication();

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            string type = request.Params["type"];
            int ticket = QS(request.Params["ticket"], 0);
            string result = string.Empty;
            switch (type)
            {
                case "delete":
                    int feedbackID = QS(request.Params["FeedbackID"], 0);
                    int userID = QS(request.Params["UID"], 0);
                    result = DeleteFeedback(userID, feedbackID, ticket);
                    break;
                case "add":
                    int project = QS(request.Params["project"], 0);
                    string content = request.Params["content"].Replace("<@script>", "</script>");
                    bool isPublic = false;
                    bool.TryParse(request.Params["isPublic"], out isPublic);

                    bool isWaitSunnetFeedback = false;
                    bool.TryParse(request.Params["isWaitSunnetFeedback"], out isWaitSunnetFeedback);
                    string uploadFile = request.Params["uloadFile"];
                    var clients = request.Params["clients"];
                    bool isWaitClientFeedback = !String.IsNullOrEmpty(clients);
                    result = AddFeedback(project, ticket, content, isPublic, isWaitClientFeedback, isWaitSunnetFeedback,
                        uploadFile, clients, context.Server);
                    break;
                default:
                    result = ResponseMessage.GetResponse(false);
                    break;
            }
            response.Write(result);
        }

        public string DeleteFeedback(int userID, int feedbackID, int ticketID)
        {
            if (userID != UserID)
            {
                return ResponseMessage.GetResponse(false, "Unauthorized operation.", 0);
            }
            if (feedbackID == 0)
            {
                return ResponseMessage.GetResponse(false, "Arguments error.", 0);
            }
            FeedBackApplication feedBackApplication = new FeedBackApplication();

            //先判定feedback是不是public然后判断是不是wait client==1或waitpm ==1如果是并且ticket状态不为normal则改为normal
            FeedBacksEntity feedBacksEntity = feedBackApplication.GetFeedBacksEntity(feedbackID);
            if (feedBacksEntity.WaitClientFeedback == FeedbackReplyStatus.Requested)
            {
                ticketApp.ClearWaitingByType(ticketID, TicketUsersType.Create, TicketUsersType.Client, TicketUsersType.PM);
                ticketApp.DeleteUserFromTicket(ticketID, TicketUsersType.Client);
            }
            if (feedBacksEntity.WaitPMFeedback == FeedbackReplyStatus.Requested)
            {
                var ticketEntity = ticketApp.GetTickets(ticketID);
                var projectEntity = projectApp.Get(ticketEntity.ProjectID);
                if (ticketApp.UpdateTicketStatus(ticketID, UserTicketStatus.Normal,
                    TicketUsersType.PM, TicketUsersType.Create, TicketUsersType.Client))
                {
                    ticketEntity.ResponsibleUser = PmReplyClient(projectEntity, ticketEntity);
                    ticketApp.UpdateTickets(ticketEntity, false);
                }
            }

            //再删除feedbackmessage,并且feedback状态不为normal则改为normal
            bool result = feedBackApplication.DeleteFeedback(feedbackID);
            if (result)
            {
                return ResponseMessage.GetResponse(true);
            }
            else
            {
                var tmpMessage = feedBackApplication.BrokenRuleMessages != null &&
                                 feedBackApplication.BrokenRuleMessages.Count > 0
                    ? feedBackApplication.BrokenRuleMessages[0].Message
                    : "";
                return ResponseMessage.GetResponse(false, tmpMessage);
            }
        }

        private delegate void SendHandler();

        public string AddFeedback(int projectID, int ticketID, string content, bool isPublic, bool isWaitClient,
            bool isWaitSunnetFeedback, string file, string clientsIds, HttpServerUtility Server)
        {
            try
            {
                ProjectsEntity ProjectEntity = projectApp.Get(projectID);
                FeedBacksEntity feedbacksEntity = GetFeedbacksEntity(ticketID, content, isPublic, isWaitClient, isWaitSunnetFeedback);
                TicketsEntity ticket = ticketApp.GetTickets(ticketID);

                var canAddWaiting = ticketApp.CanFeedbackWaiting(ticketID);
                if (!canAddWaiting)
                    feedbacksEntity.WaitPMFeedback = feedbacksEntity.WaitClientFeedback = FeedbackReplyStatus.Normal;

                string fileuploadErrMsg = string.Empty;

                feedbacksEntity.ID = fbAPP.AddFeedBacks(feedbacksEntity);
                if (feedbacksEntity.ID <= 0)
                {
                    var msg = fbAPP.BrokenRuleMessages != null && fbAPP.BrokenRuleMessages.Count > 0
                        ? fbAPP.BrokenRuleMessages[0].Message
                        : "";
                    return ResponseMessage.GetResponse(false, msg);
                }

                // 产生气泡提示
                if (UserInfo.Role == RolesEnum.CLIENT || isPublic)
                    ticketApp.CreateNotification(ticketID, UserInfo.UserID, false);
                else
                    ticketApp.CreateNotification(ticketID, UserInfo.UserID);

                var extraStatus = new Dictionary<string, bool>();
                // 在添加FeedbackMessage时要更新Ticket 对相关人员的状态, 并且不能覆盖状态

                if (ticket.Status != TicketsState.Wait_Sunnet_Feedback &&
                    ticket.Status != TicketsState.Wait_Client_Feedback &&
                    canAddWaiting)
                {
                    if ((UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales) && isWaitClient)
                    {
                        ticketApp.UpdateTicketStatus(ticketID, UserTicketStatus.WaitClientFeedback, UserID);
                        extraStatus.Add("waitSunnet", ticketApp.UpdateTicketStatus(ticketID, UserTicketStatus.WaitClientFeedback, TicketUsersType.Create));
                        new SendHandler(() => ticketStatusMgr.SendEmailtoClientForFeedBack(feedbacksEntity))
                      .BeginInvoke(null, null);

                        ProcessOtherClients(clientsIds, ticketID);
                    }
                    else if (UserInfo.Role == RolesEnum.CLIENT)
                    {
                        if (isWaitSunnetFeedback)
                        {

                            ticketApp.UpdateTicketStatus(ticketID, UserTicketStatus.WaitSunnetFeedback, UserID);
                            extraStatus.Add("waitClient", ticketApp.UpdateTicketStatus(ticketID, UserTicketStatus.WaitSunnetFeedback, TicketUsersType.PM));
                        }
                        new SendHandler(() => ticketStatusMgr.SendEmailtoPMForFeedBack(feedbacksEntity))
                      .BeginInvoke(null, null);
                    }
                }
                else
                {
                    // 相关状态人员可以通过回复清除自己的状态标识
                    if ((UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales) && ticket.Status == TicketsState.Wait_Sunnet_Feedback && isPublic)
                    {
                        // Sunnet 直接清除PM的标识
                        if (ticketApp.UpdateTicketStatus(ticketID, UserTicketStatus.Normal, TicketUsersType.PM) &&
                            fbAPP.ReplyFeedback(ticketID, true, false))
                        {
                            ticketApp.UpdateTicketStatus(ticketID, UserTicketStatus.Normal, TicketUsersType.Create,
                                TicketUsersType.Client);
                            ticket.ResponsibleUser = PmReplyClient(ProjectEntity, ticket);
                            extraStatus.Add("clearSunnet", true);
                            var createrId = ticketApp.GetTicketCreateUser(ticketID);
                            var creater = _userApp.GetUser(createrId.UserID);
                            new SendHandler(() => ticketStatusMgr.SendEmailFeedbackReplied(UserInfo, creater, ticketID))
                                .BeginInvoke(null, null);
                        }

                    }
                    if (UserInfo.Role == RolesEnum.CLIENT && ticket.Status == TicketsState.Wait_Client_Feedback)
                    {
                        // 客户方面只能清除自己的标识,因为可能需要多个客户回复
                        if (ticketApp.TryClearWaiting(ticketID, UserID, TicketUsersType.Create, TicketUsersType.Client) &&
                            fbAPP.ReplyFeedback(ticketID, false, true))
                        {
                            ticketApp.UpdateTicketStatus(ticketID, UserTicketStatus.Normal, TicketUsersType.PM);
                            extraStatus.Add("clearClient", true);
                            var pmId = ticketApp.GetTicketPM(ticketID);
                            var pm = _userApp.GetUser(pmId.UserID);
                            new SendHandler(() => ticketStatusMgr.SendEmailFeedbackReplied(UserInfo, pm, ticketID))
                                .BeginInvoke(null, null);

                            ticketApp.DeleteUserFromTicket(ticketID, TicketUsersType.Client);
                        }

                    }
                }
                ticket.ModifiedOn = DateTime.Now;
                if (UserInfo.Role == RolesEnum.CLIENT && isWaitSunnetFeedback)
                {
                    ChangeResponsUserToProjectManager(ProjectEntity, ticket);
                }

                ticketApp.UpdateTickets(ticket, false);

                #region add file

                int fileID = 0;
                string realFileName = "";
                string[] files = file.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, int> uploadedFiles = new Dictionary<string, int>();
                foreach (var f in files)
                {
                    if ((fileID =
                       InsertFile(f, ProjectEntity.ProjectID, ticketID, feedbacksEntity.IsPublic,
                           feedbacksEntity.ID, Server, out realFileName)) < 1)
                        fileuploadErrMsg += realFileName + " Upload failed.";
                    uploadedFiles.Add(realFileName, fileID);
                }

                #endregion

                if (string.IsNullOrEmpty(fileuploadErrMsg))
                {
                    ResponseMessage response = new ResponseMessage();
                    response.Success = true;

                    object attachement = null;
                    object attachementBasicInfo = null;
                    if (uploadedFiles.Count > 0)
                    {
                        attachement = uploadedFiles;
                        string[] imgFormat = ".gif,.jpg,.jpeg,.bmp,.png,.svg,.raw".Split(new char[] { ',' });
                        attachementBasicInfo = uploadedFiles.Select(it => new
                        {
                            Key = it.Key,
                            Value = imgFormat.Any(format => format == Path.GetExtension(it.Key.ToLower())) ? fileApp.Get(it.Value).FilePath : "/Images/icons/file.png"
                        }).ToDictionary(it => it.Key, it => it.Value);
                    }

                    feedbacksEntity.Description = new HtmlHelper().ReplaceUrl(feedbacksEntity.Description);

                    response.Data = new
                    {
                        content = feedback.FormatHTML(feedbacksEntity.Description, Server).Replace("\n", "<br/>"),
                        id = feedbacksEntity.ID,
                        date = feedbacksEntity.CreatedOn.ToString("MM/dd/yyyy"),
                        time = feedbacksEntity.CreatedOn.ToString("hh:mm tt"),
                        order = feedbacksEntity.Order,
                        createdBy = feedbacksEntity.CreatedBy,
                        firstname = UserInfo.FirstName,
                        lastname = UserInfo.LastName,
                        file = attachement,
                        basicInfo = attachementBasicInfo,
                        extraStatus = extraStatus,
                        tdCss = feedbacksEntity.IsPublic && UserInfo.Role == RolesEnum.PM ? "fdcontentBox3" : "fdcontentBox2"
                    };

                    return response.ToString();
                }
                else
                {
                    return ResponseMessage.GetResponse(false, fileuploadErrMsg);
                }
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(string.Format("Error Ascx:FeedbackList.ascx Messages:\r\n{0}", ex));
                return ResponseMessage.GetResponse(false, ex.Message);
            }
        }

        private void ProcessOtherClients(string clientsIds, int ticketID)
        {
            if (string.IsNullOrEmpty(clientsIds))
                return;
            var clients = clientsIds.Split(",".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);
            if (clients == null || !clients.Any())
                return;
            var clientsToAssign = clients.Select(r => int.Parse(r));
            var creatorTicketuser = ticketApp.GetTicketUser(ticketID, TicketUsersType.Create);
            clientsToAssign = clientsToAssign.Except(creatorTicketuser.Select(x => x.UserID));
            if (ticketApp.AssignUsers(ticketID, TicketUsersType.Client, clientsToAssign.ToArray())
                && ticketApp.UpdateTicketStatus(ticketID, UserTicketStatus.WaitClientFeedback, clientsToAssign.ToArray()))
            {
                new SendHandler(
                    () => ticketStatusMgr.SendEmailtoClientForFeedBack(ticketID, clientsToAssign.ToArray()))
                    .BeginInvoke(null, null);
            }
        }

        private int InsertFile(string file, int projectID, int ticketID, bool isPublicFeedback, int feedbacksID, HttpServerUtility Server, out string realFileName)
        {
            realFileName = file.Replace(string.Format("{0}_", UserID), "");
            var fileLength = (int)FileHelper.GetFileLength(file);
            string tempPath = Config.UploadPath;
            string folderName = projectID.ToString();
            string fileName = file;
            string savePath = Path.Combine(Server.MapPath(tempPath), folderName);
            string sExtension = Path.GetExtension(fileName);
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            string sNewFileName = string.Format("{0}_{1}_{2}{3}", ticketID, Path.GetFileNameWithoutExtension(file), DateTime.Now.ToString("ssmmfff"), sExtension);

            FilesEntity fileEntity = new FilesEntity();
            fileEntity.ContentType = sExtension.ToLower();
            fileEntity.CreatedBy = UserInfo.UserID;
            fileEntity.FilePath = string.Format("{0}{1}/{2}", tempPath.Substring(1), folderName, sNewFileName);
            fileEntity.FileSize = Convert.ToDecimal(fileLength);
            fileEntity.FileTitle = realFileName.Substring(0, realFileName.LastIndexOf('.'));
            fileEntity.IsPublic = isPublicFeedback;
            fileEntity.FeedbackId = feedbacksID;
            fileEntity.TicketId = ticketID;
            fileEntity.ProjectId = projectID;
            fileEntity.SourceType = (int)FileSourceType.FeedBack;
            fileEntity.ThumbPath = "";
            fileEntity.CreatedOn = DateTime.Now.Date;
            fileEntity.CompanyID = ticketApp.GetCompanyIdByTicketId(ticketID);

            if (FileHelper.Move(file, Server.MapPath(fileEntity.FilePath)))
            {
                return fileApp.AddFile(fileEntity);
            }
            return 0;
        }

        private FeedBacksEntity GetFeedbacksEntity(int ticketID, string content, bool isPublic, bool isWaitClient, bool isWaitPM)
        {
            FeedBacksEntity feedbacksEntity = new FeedBacksEntity();
            feedbacksEntity.IsDelete = false;
            feedbacksEntity.TicketID = ticketID;
            feedbacksEntity.Title = string.Empty;
            feedbacksEntity.Description = content; //content.NoHTML();
            feedbacksEntity.CreatedBy = UserInfo.UserID;
            feedbacksEntity.CreatedOn = DateTime.Now;
            feedbacksEntity.ModifiedOn = DateTime.Now;

            feedbacksEntity.WaitClientFeedback = isWaitClient ? FeedbackReplyStatus.Requested : FeedbackReplyStatus.Normal;
            feedbacksEntity.WaitPMFeedback = isWaitPM ? FeedbackReplyStatus.Requested : FeedbackReplyStatus.Normal;

            if (UserInfo.Role == RolesEnum.CLIENT || isWaitClient)
            {
                feedbacksEntity.IsPublic = true;
            }
            else
                feedbacksEntity.IsPublic = isPublic;

            return feedbacksEntity;
        }

        /// <summary>
        ///  改变ResponsUser为项目负责人(一般是pm)
        /// </summary>
        private bool ChangeResponsUserToProjectManager(ProjectsEntity projectEntity, TicketsEntity ticketEntity)
        {
            var newTicket = ticketApp.GetTickets(ticketEntity.TicketID);
            string description = "Change status to " + newTicket.Status.ToText() + ".";

            if (projectEntity.PMID != ticketEntity.ResponsibleUser)
            {
                var oldResponseUser = _userApp.GetUser(ticketEntity.ResponsibleUser);
                var pmResponseUser = _userApp.GetUser(projectEntity.PMID);
                if (oldResponseUser != null && pmResponseUser != null)
                {
                    description += " And Change response user from " + oldResponseUser.FirstName + " " + oldResponseUser.LastName + " to " + pmResponseUser.FirstName + " " + pmResponseUser.LastName;
                }
                ticketEntity.ResponsibleUser = projectEntity.PMID;
                ticketAPP.UpdateTicket(ticketEntity);
            }
            //添加历史记录
            TicketHistorysEntity historysEntity = new TicketHistorysEntity();
            historysEntity.Description = description;
            historysEntity.ModifiedBy = IdentityContext.UserID;
            historysEntity.CreatedOn = DateTime.Now;
            historysEntity.ModifiedOn = DateTime.Now;
            historysEntity.TicketID = ticketEntity.ID;
            historysEntity.TDHID = 0;
            historysEntity.ResponsibleUserId = projectEntity.PMID;

            if (ticketAPP.AddTicketHistory(historysEntity) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int PmReplyClient(ProjectsEntity projectEntity, TicketsEntity ticketEntity)
        {
            var historys = ticketAPP.GetHistoryListByTicketID(ticketEntity.ID);
            int previousResponseUserId = 0;
            if (historys.Count >= 2)
            {
                previousResponseUserId = historys[1].ResponsibleUserId;
            }
            else if (historys.Count >= 1)
            {
                previousResponseUserId = historys[0].ResponsibleUserId;
            }
            else
            {
                previousResponseUserId = projectEntity.PMID;
            }

            string description = "Change status to " + ticketEntity.RealStatus.ToText() + ".";

            if (previousResponseUserId != ticketEntity.ResponsibleUser)
            {
                var oldResponseUser = _userApp.GetUser(ticketEntity.ResponsibleUser);
                var pmResponseUser = _userApp.GetUser(previousResponseUserId);
                if (oldResponseUser != null && pmResponseUser != null)
                {
                    description += " And Change response user from " + oldResponseUser.FirstName + " " + oldResponseUser.LastName + " to " + pmResponseUser.FirstName + " " + pmResponseUser.LastName;
                }
                ticketEntity.ResponsibleUser = previousResponseUserId;
                ticketAPP.UpdateTicket(ticketEntity);
            }

            //添加历史记录
            TicketHistorysEntity historysEntity = new TicketHistorysEntity();
            historysEntity.Description = description;
            historysEntity.ModifiedBy = IdentityContext.UserID;
            historysEntity.CreatedOn = DateTime.Now;
            historysEntity.ModifiedOn = DateTime.Now;
            historysEntity.TicketID = ticketEntity.ID;
            historysEntity.TDHID = 0;
            historysEntity.ResponsibleUserId = previousResponseUserId;

            ticketAPP.AddTicketHistory(historysEntity);

            return previousResponseUserId;
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