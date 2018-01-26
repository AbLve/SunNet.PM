using System.IO;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.App;
using System.Transactions;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel.UserModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.PM2014.Ticket.Sunnet
{
    public partial class QAReview : BasePage
    {
        public int TicketID { get; set; }
        TicketsApplication ticketApp = new TicketsApplication();
        FeedBackApplication fbAPP = new FeedBackApplication();
        FileApplication fileApp = new FileApplication();
        TicketsEntity _ticketEntity;
        ProjectApplication projectApp = new ProjectApplication();

        private bool IsETA
        {
            get
            {
                return _ticketEntity != null
                    && _ticketEntity.IsEstimates
                    && _ticketEntity.Status == TicketsState.Waiting_For_Estimation
                    && _ticketEntity.EsUserID == UserInfo.UserID;
            }
        }
        private List<ListItem> NextStates
        {
            get
            {
                return ConvertStateListToItemList(IsETA ? ticketApp.DevEstimationNext : ticketApp.QaNextStates);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ((Pop)this.Master).Width = 650;
            TicketID = QS("tid", 0);
            _ticketEntity = ticketApp.GetTickets(TicketID);
            fileUploader.TicketID = TicketID;
            if (!IsPostBack)
            {
                if (_ticketEntity != null)
                {
                    if (IsETA)
                    {
                        phlETA.Visible = true;
                    }
                    ddlStatus.Items.AddRange(NextStates.ToArray());
                    string selectedValue = string.Empty;
                    switch (_ticketEntity.Status)
                    {
                        case TicketsState.Testing_On_Local:
                            {
                                selectedValue = ((int)(TicketsState.Tested_Success_On_Local)).ToString();
                                break;
                            }

                        case TicketsState.Testing_On_Client:
                            {
                                selectedValue = ((int)(TicketsState.Tested_Success_On_Client)).ToString();
                                break;
                            }
                    }
                    litHead.Text = "Ticket ID: " + _ticketEntity.TicketID + ", " + _ticketEntity.Title;
                    ddlStatus.SelectedValue = selectedValue;
                    ResponseUserBind();
                }

            }
        }

        private void ResponseUserBind()
        {
            List<SelectUserModel> ticketUsers = ticketApp.GetSelectUsersByTicketId(TicketID);
            ticketUsers = ticketUsers.Where(x => x.Role != RolesEnum.CLIENT).ToList();
            ddlResponsibleUser.DataSource = ticketUsers;
            ddlResponsibleUser.DataValueField = "UserID";
            ddlResponsibleUser.DataTextField = "UserName";
            ddlResponsibleUser.DataBind();
            ListItem item = new ListItem("System", "-1");
            ddlResponsibleUser.Items.Insert(0, item);
            int responsibleUserId = _ticketEntity.ResponsibleUser;
            int selectIndex = ticketUsers.FindIndex(x => x.UserID == responsibleUserId);
            ddlResponsibleUser.SelectedIndex = selectIndex + 1;
        }

        private FeedBacksEntity GetFeedbacksEntity()
        {
            var feedbacksEntity = new FeedBacksEntity
            {
                IsDelete = false,
                TicketID = TicketID,
                Title = string.Empty,
                Description = txtDescription.Text,
                CreatedBy = UserInfo.UserID,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                IsPublic = false,
                WaitClientFeedback = FeedbackReplyStatus.Normal,
                WaitPMFeedback = FeedbackReplyStatus.Normal
            };
            return feedbacksEntity;
        }
        private bool InsertFile(HttpPostedFile file, int projectID, bool isPublicFeedback, int feedbacksID)
        {
            string tempPath = System.Configuration.ConfigurationManager.AppSettings["FolderPath"];
            string folderName = string.Empty;
            folderName = projectID.ToString();
            string fileName = file.FileName;
            string savePath = Server.MapPath(tempPath) + folderName;
            string sExtension = Path.GetExtension(fileName);
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            string sNewFileName = string.Format("{0}_{1}{2}", feedbacksID, DateTime.Now.ToString("yyMMddssmm"), sExtension); ;
            file.SaveAs(savePath + @"\" + sNewFileName);

            FilesEntity fileEntity = new FilesEntity();
            fileEntity.ContentType = sExtension.ToLower();
            fileEntity.CreatedBy = UserInfo.UserID;
            fileEntity.FilePath = tempPath.Substring(2) + folderName + @"/" + sNewFileName;
            fileEntity.FileSize = Convert.ToDecimal(file.ContentLength);
            fileEntity.FileTitle = fileName.Substring(0, fileName.LastIndexOf('.'));
            fileEntity.IsPublic = isPublicFeedback;
            fileEntity.FeedbackId = feedbacksID;
            fileEntity.TicketId = TicketID;
            fileEntity.ProjectId = projectID;
            fileEntity.SourceType = (int)FileSourceType.FeedBack;
            fileEntity.ThumbPath = "";
            fileEntity.CreatedOn = DateTime.Now.Date;
            fileEntity.CompanyID = ticketApp.GetCompanyIdByTicketId(TicketID);
            return fileApp.AddFile(fileEntity) > 0;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //保存状态。
            //如果需要估时则保存估时
            bool result = true;
            if (IsETA)
            {
                decimal etahours = 0;
                if (decimal.TryParse(txtBoxExtimationHours.Text, out etahours))
                {
                    _ticketEntity.InitialTime = etahours;
                }
            }
            if (!result)
            {
                ShowFailMessageToClient(ticketApp.BrokenRuleMessages);
                return;
            }
            var currentState = ddlStatus.SelectedValue.ToEnum<TicketsState>();
            _ticketEntity.Status = currentState;
            _ticketEntity.ModifiedBy = UserInfo.UserID;
            _ticketEntity.ModifiedOn = DateTime.Now;
            int oldResponsibleUserId = _ticketEntity.ResponsibleUser;
            ProjectsEntity projectEntity = projectApp.Get(_ticketEntity.ProjectID);
            _ticketEntity.ResponsibleUser = int.Parse(ddlResponsibleUser.SelectedValue);

            result = ticketApp.UpdateTickets(_ticketEntity);
            if (result)
            {
                //sent email to responsible user 2017/10/23
                if (oldResponsibleUserId != _ticketEntity.ResponsibleUser)
                {
                    ticketApp.SendEmailToResponsibile(_ticketEntity, UserInfo);
                }
            }
            if (!result)
            {
                ShowFailMessageToClient(ticketApp.BrokenRuleMessages);
                return;
            }
            if (!IsETA && (currentState == TicketsState.Tested_Fail_On_Local || currentState == TicketsState.Tested_Fail_On_Client))
            {
                FilesEntity fileEntity = new FilesEntity();
                HttpFileCollection files = Request.Files;
                string fileuploadErrMsg = string.Empty;
                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].ContentLength > 0 || txtDescription.Text.Trim().Length > 0 && IsValidFile(files[i].FileName))
                    {

                        FeedBacksEntity feedbacksEntity = GetFeedbacksEntity();
                        feedbacksEntity.ID = fbAPP.AddFeedBacks(feedbacksEntity);
                        result = feedbacksEntity.ID > 0;
                        if (!result)
                        {
                            ShowFailMessageToClient(fbAPP.BrokenRuleMessages);
                            return;
                        }
                        if (files[i].ContentLength > 0 && IsValidFile(files[i].FileName))
                        {
                            if (!InsertFile(files[i], _ticketEntity.ProjectID, feedbacksEntity.IsPublic,
                                    feedbacksEntity.ID))
                            {
                                fileuploadErrMsg += files[i].FileName + " Upload failed.";
                                result = false;
                            }
                        }
                    }
                }
                if (!result)
                {
                    ShowFailMessageToClient(fileuploadErrMsg);
                    return;
                }
            }
            if (result)
                Redirect(EmptyPopPageUrl, false, true);
            else
                ShowFailMessageToClient(ticketApp.BrokenRuleMessages);

        }
    }
}