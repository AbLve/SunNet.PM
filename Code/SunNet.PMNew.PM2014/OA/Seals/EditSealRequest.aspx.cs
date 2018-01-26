using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.PM2014.Codes;
using System.Web.UI.WebControls;
using System.Drawing;
using SunNet.PMNew.Entity.SealModel.Enum;
using System.Data;
using System.Web.UI;
using StructureMap;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.PM2014.OA.Seals
{
    public partial class EditSealRequest : BasePage
    {
        protected SealRequestsEntity sealRequestEntity;
        protected bool Disable;
        SealsApplication app = new SealsApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (QS("id", 0) > 0) //edit
                {
                    sealRequestEntity = app.GetSealRequests(QS("Id", 0));
                    BindRequestData(sealRequestEntity);
                    hdID.Value = sealRequestEntity.ID.ToString();

                    // Check authorization
                    if (app.CheckUserHasRecords(UserInfo.ID, sealRequestEntity.ID, (int)WorkflowAction.Pending)
                        || (sealRequestEntity.Status == RequestStatus.Draft && sealRequestEntity.RequestedBy == UserInfo.ID)) // Still a draft
                    {
                        divAction.Style["display"] = "inline";
                        BindDdlAction();
                    }

                }
            }
        }

        private void BindDdlAction()
        {
            ddlAction.Items.Add(new ListItem() { Text = "Please select action", Value = "-1" });
            switch (sealRequestEntity.Status)
            {
                case RequestStatus.Denied:
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.Cancel.WorkflowActionToText(), Value = WorkflowAction.Cancel.ToString() });
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.Submit.WorkflowActionToText(), Value = WorkflowAction.Submit.ToString() });
                    break;
                case RequestStatus.Draft:
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.Save.WorkflowActionToText(), Value = WorkflowAction.Save.ToString() });
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.Cancel.WorkflowActionToText(), Value = WorkflowAction.Cancel.ToString() });
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.Submit.WorkflowActionToText(), Value = WorkflowAction.Submit.ToString() });
                    break;
                case RequestStatus.Submitted:
                case RequestStatus.PendingApproval:
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.ForwardApproval.WorkflowActionToText(), Value = WorkflowAction.ForwardApproval.ToString() });
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.Approve.WorkflowActionToText(), Value = WorkflowAction.Approve.ToString()});
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.Deny.WorkflowActionToText(), Value = WorkflowAction.Deny.ToString() });                    
                    break;
                case RequestStatus.Approved:
                case RequestStatus.PendingProcess:
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.ForwardProcess.WorkflowActionToText(), Value = WorkflowAction.ForwardProcess.ToString() });
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.FinishProcess.WorkflowActionToText(), Value = WorkflowAction.FinishProcess.ToString() });
                    break;
                case RequestStatus.Processed:
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.ContinueProcess.WorkflowActionToText(), Value = WorkflowAction.ContinueProcess.ToString() });
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.Complete.WorkflowActionToText(), Value = WorkflowAction.Complete.ToString() });
                    ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.Cancel.WorkflowActionToText(), Value = WorkflowAction.Cancel.ToString() });
                    break;

            }
        }

        private string BuilderSeals(int sealRequestId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='0' ><tbody>");

            List<SealsEntity> list = app.GetList().FindAll(r => r.Status == Status.Active);
            List<SealUnionRequestsEntity> listUnioSeals = app.GetSealUnionRequestsList(sealRequestId);

            foreach (SealsEntity entity in list)
            {
                SealUnionRequestsEntity unionEntity = listUnioSeals.Find(r => r.SealID == entity.ID);
                if (unionEntity != null)
                {
                    sb.AppendFormat("<tr><td><span disabled=\"disabled\"><input type=\"checkbox\" disabled=\"disabled\" {0}> <label>{1}</label></span>",
                        unionEntity == null ? "" : "checked=\"checked\"", entity.SealName);

                    if (unionEntity != null && unionEntity.SealedDate > MinDate)
                        sb.AppendFormat("<span style='margin-left:20px'><span class=\"sealinfo\">Sealed By:</span> {0} <span class=\"sealinfo\">Sealed Date:</span> {1} </span>",
                            new App.UserApplication().GetUser(unionEntity.SealedBy).FirstName, unionEntity.SealedDate.ToString("MM/dd/yyyy"));
                    sb.Append("</td></tr>");
                }
            }
            sb.Append("</tbody></table>");
            return sb.ToString();
        }

        private void BindRequestData(SealRequestsEntity entity)
        {
            hdID.Value = entity.ID.ToString();
            txtTitle.Text = entity.Title;
            lblStatus.Text = entity.Status.RequestStatusToText();
            lblCreator.Text = new App.UserApplication().GetUser(entity.RequestedBy).FirstAndLastName;
            txtDescription.Text = entity.Description.Replace("<br>", "\r\n");

            List<SealUnionRequestsEntity> list = app.GetSealUnionRequestsList(entity.ID);

            if (entity.Type == 0)
            {
                if ((int)entity.Status < (int)RequestStatus.Processed)
                {
                    List<SealsEntity> seallist = app.GetList().FindAll(r => r.Status == Status.Active);
                    foreach (SealUnionRequestsEntity tmpEntity in list)
                    {
                        SealsEntity tmpSealEntity = seallist.Find(r => r.ID == tmpEntity.SealID);
                        if (tmpSealEntity != null)
                            tmpSealEntity.Checked = true;
                    }
                    rptSeals.DataSource = seallist;
                    rptSeals.DataBind();
                }
                else
                {
                    ltSelas.Text = BuilderSeals(entity.ID);
                }
            }

            if (entity.RequestedBy == UserInfo.UserID && entity.Status < RequestStatus.Approved) //修改为在Approve前都可以删除
                lblFiles.InnerHtml = BuilderFiles(entity.ID, true);
            else
                lblFiles.InnerHtml = BuilderFiles(entity.ID, false);

            if (entity.Status == RequestStatus.Draft && UserInfo.UserID == entity.RequestedBy)
            {
                DisableControl(false, string.Empty);
                divUploadFiles1.Visible = true;
            }
            else
            {
                DisableControl(true, entity.Description);
                divUploadFiles1.Visible = false;
            }

            if (sealRequestEntity.Status == RequestStatus.Canceled || sealRequestEntity.Status == RequestStatus.Completed || sealRequestEntity.Status == RequestStatus.Draft)
            {
                divUploadFiles2.Visible = false;
                divComments.Visible = false;
            }
            else
            {
                divUploadFiles2.Visible = true;
                divComments.Visible = true;
            }

            rptFiles.DataSource = app.GetSealFilesList(entity.ID, -1);
            rptFiles.DataBind();

            rptNotes.DataSource = app.GetSealNotesList(entity.ID);
            rptNotes.DataBind();

            // Bind Workflow history Repeater
            List<WorkflowHistoryEntity> wfhisLst = app.GetWorkflowHistoryList(entity.ID);
            rptWorkflowHistory.DataSource = wfhisLst;
            rptWorkflowHistory.DataBind();
        }

        /// <summary>
        /// 禁用控件
        /// </summary>
        /// <param name="disable">是否禁用</param>
        /// <param name="description"></param>
        private void DisableControl(bool disable, string description)
        {
            Disable = disable;
            if (disable)
            {
                txtTitle.Enabled = false;
                txtDescription.Visible = false;
                lblDescription.InnerHtml = description;
            }
        }

        private string BuilderFiles(int requestID, bool showDeleted)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"fileList\">");

            List<SealFileEntity> list = app.GetSealFilesList(requestID, 0);
            foreach (SealFileEntity entity in list)
            {
                sb.AppendFormat("<li id=\"lif{0}\"><a href=\"/do/DownloadSealFile.ashx?id={0}\" target=\"_blank\">» {1}</a>", entity.ID, entity.Name);
                if (showDeleted)
                    sb.AppendFormat("&nbsp;&nbsp;<a href='javascript:void(0);' onclick='deleteFile(\"{0}\");'><img src='/Images/ico_delete.gif' align='absmiddle'/></a>"
                        , entity.ID);
                sb.Append("</li>");
            }
            sb.Append("</ul>");

            return sb.ToString();
        }

        private void InsertFile(HttpPostedFile file, int sealRequestId, int wfhisID, int index)
        {
            string fileName = file.FileName;
            string tmpFileName = string.Format("{0}{2}{1}", DateTime.Now.ToString("MMddyyHHmmss"), fileName.Substring(fileName.LastIndexOf(".")), index);
            file.SaveAs(Config.SealFilePath + tmpFileName);
            SealFileEntity fileEntity = new SealFileEntity();
            fileEntity.Title = string.Empty;
            fileEntity.Name = fileName;
            fileEntity.Path = Config.SealFilePath + tmpFileName;
            fileEntity.SealRequestsID = sealRequestId;
            fileEntity.UserID = UserInfo.UserID;
            fileEntity.WorkflowHistoryID = wfhisID;
            fileEntity.IsDeleted = false;
            fileEntity.CreateOn = DateTime.Now;
            app.SealFilesInsert(fileEntity);
        }

        private SealRequestsEntity CheckData()
        {
            int id;
            if (int.TryParse(hdID.Value, out id))
            {
                SealRequestsEntity sealRequestsEntity = app.GetSealRequests(id);
                if (sealRequestsEntity == null)
                {
                    ShowFailMessageToClient("unauthorized access.");
                    return null;
                }
                return sealRequestsEntity;
            }
            else
            {
                ShowFailMessageToClient("unauthorized access.");
                return null;
            }
        }

        protected string ShowDeleteButton(int id)
        {
            SealFileEntity sealFileEntity = app.GetSealFiles(id);
            SealRequestsEntity sealRequestsEntity = app.GetSealRequests(sealFileEntity.SealRequestsID);
            if (sealFileEntity.UserID != UserInfo.UserID || sealRequestsEntity.Status >= RequestStatus.Approved)
            {
                return "&nbsp;";
            }
            else
            {
                SealsEntity sealsEntity = app.Get(sealFileEntity.SealRequestsID);
                return string.Format("<a href=\"javascript:void(0);\" onclick=\"deleteFile('{0}',true,this)\"><img border=\"0\" title=\"Delete\" src=\"/Images/icons/delete.png\"></a>"
                   , id);
            }
        }


        private void ChangeRequestStatus_Canceled()
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            if (sealRequestsEntity != null)
            {
                if (sealRequestsEntity.RequestedBy != UserInfo.UserID)
                {
                    ShowFailMessageToClient("unauthorized access.");
                    return;
                }
                if (app.UpdateStatus(sealRequestsEntity.ID, RequestStatus.Canceled))
                {
                }
                else
                    ShowFailMessageToClient(app.BrokenRuleMessages);
            }
        }

        /// <summary>
        /// Submit
        /// </summary>
        private void ChangeRequestStatus_Submitted()
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            if (sealRequestsEntity != null)
            {
                if ((sealRequestsEntity.Status != RequestStatus.Draft && sealRequestsEntity.Status !=  RequestStatus.Denied) || sealRequestsEntity.RequestedBy != UserInfo.UserID)
                {
                    ShowFailMessageToClient("unauthorized access.");
                    return;
                }
                if (SaveData(sealRequestsEntity))
                {
                    SendSubmitEmail(sealRequestsEntity);
                }
            }
        }

        private void SendSubmitEmail(SealRequestsEntity sealRequestsEntity)
        {
            if (app.UpdateStatus(sealRequestsEntity.ID, RequestStatus.Submitted))
            {
                string mailTemplatePath = Server.MapPath(@"~\Template\SendEmailToApproved.txt");
                string mailTemplate = File.ReadAllText(mailTemplatePath);
                string mailTitle = "";
                if (sealRequestEntity.Type == 0)
                    mailTitle = "[Seal] " + sealRequestsEntity.Title;
                else if (sealRequestEntity.Type == 1)
                {
                    mailTitle = "[WorkOrder] " + sealRequestsEntity.Title;
                }

                foreach (SealUnionRequestsEntity unionEntity in app.GetApprovedByList(sealRequestsEntity.ID))
                {
                    string content = mailTemplate.Replace("[ClientName]", unionEntity.FirstName).Replace("[applicant]", UserInfo.FirstName)
                         .Replace("[content]", sealRequestsEntity.Description);
#if !DEBUG
                    ObjectFactory.GetInstance<IEmailSender>().SendMail(unionEntity.Email, Config.DefaultSendEmail, mailTitle, content);
#endif
                }
            }
            else
                ShowFailMessageToClient(app.BrokenRuleMessages);
        }

        private void ChangeRequestStatus_ForwardApproval()
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            if (sealRequestsEntity != null)
            {
                //app.UpdateApprovedDate(sealRequestsEntity.ID, UserInfo.UserID, DateTime.Now);
                app.UpdateStatus(sealRequestsEntity.ID, RequestStatus.PendingApproval);
            }
            else
                ShowFailMessageToClient("unauthorized access.");
        }

        /// <summary>
        /// Approved
        /// </summary>
        private void ChangeRequestStatus_Approved()
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            if (sealRequestsEntity != null)
            {
                //List<SealUnionRequestsEntity> list = app.GetSealUnionRequestsList(sealRequestsEntity.ID);
                if (app.SealRequestApproved(sealRequestsEntity.ID, UserInfo.ID))
                {

                    string mailTemplatePath = Server.MapPath(@"~\Template\SendEmailToSealed.txt");
                    string mailTemplate = File.ReadAllText(mailTemplatePath);
                    string mailTitle = "[Approved] " + sealRequestsEntity.Title;

                    UsersEntity usersEntity = new App.UserApplication().GetUser(sealRequestsEntity.RequestedBy);

                    foreach (SealUnionRequestsEntity user_unionEntity in app.GetSealedByList(sealRequestsEntity.ID))
                    {
                        string content = mailTemplate.Replace("[ClientName]", user_unionEntity.FirstName).Replace("[applicant]", usersEntity.FirstName)
                            .Replace("[content]", sealRequestsEntity.Description);
#if !DEBUG
                        new SmtpClientEmailSender(new TextFileLogger()).SendMail(user_unionEntity.Email, Config.DefaultSendEmail, mailTitle, content);
#endif
                    }
                }
                else
                    ShowFailMessageToClient(app.BrokenRuleMessages);
            }
            else
                ShowFailMessageToClient("unauthorized access.");
        }

        private void ChangeRequestStatus_ForwardProcessing()
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            if (sealRequestsEntity != null)
            {
                //app.UpdateSealedDate(sealRequestsEntity.ID, DateTime.Now);
                app.UpdateStatus(sealRequestsEntity.ID, RequestStatus.PendingProcess);
            }
            else
                ShowFailMessageToClient("unauthorized access.");
        }

        /// <summary>
        /// Denied
        /// </summary>
        private void ChangeRequestStatus_Denied()
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            if (sealRequestsEntity != null)
            {
                if (app.SealRequestDenied(sealRequestsEntity.ID, UserInfo.UserID))
                {
                    string mailTemplatePath = Server.MapPath(@"~\Template\SendEmailSealDenied.txt");
                    string mailTemplate = File.ReadAllText(mailTemplatePath);
                    string mailTitle = "[Deny-" + sealRequestsEntity.ID + "] " + sealRequestsEntity.Title;

                    UsersEntity usersEntity = new App.UserApplication().GetUser(sealRequestsEntity.RequestedBy);

                    string content = mailTemplate.Replace("[ClientName]", usersEntity.FirstName)
                            .Replace("[content]", sealRequestsEntity.Description);
#if !DEBUG
                    new SmtpClientEmailSender(new TextFileLogger()).SendMail(usersEntity.Email, Config.DefaultSendEmail, mailTitle, content);
#endif
                }
                else
                    ShowFailMessageToClient(app.BrokenRuleMessages);
            }
            else
            {
                ShowFailMessageToClient("unauthorized access.");
            }
        }

        /// <summary>
        /// Seal
        /// </summary>
        private void ChangeRequestStatus_Processed()
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            if (sealRequestsEntity != null)
            {
                if (app.SealRequestSealed(sealRequestsEntity.ID, UserInfo.ID))
                {
                }
                else
                    ShowFailMessageToClient(app.BrokenRuleMessages);
            }
            else
            {
                ShowFailMessageToClient("unauthorized access.");
            }
        }

        private void ChangeRequestStatus_Completed()
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            if (sealRequestsEntity != null)
            {
                if (sealRequestsEntity.Status == RequestStatus.Processed && sealRequestsEntity.RequestedBy == UserInfo.UserID)
                {
                    if (app.UpdateStatus(sealRequestsEntity.ID, RequestStatus.Completed))
                    {
                    }
                    else
                        ShowFailMessageToClient(app.BrokenRuleMessages);
                }
                else
                {
                    ShowFailMessageToClient("unauthorized access.");
                }
            }
        }

        private bool SaveData(SealRequestsEntity sealRequestsEntity)
        {
            sealRequestsEntity.Title = txtTitle.Text;
            sealRequestsEntity.Description = txtDescription.Text.Replace("\r\n", "<br>");

            List<SealsEntity> list = app.GetList().FindAll(r => r.Status == Status.Active);
            sealRequestsEntity.SealList = new List<SealsEntity>();
            if (string.IsNullOrEmpty(QF("chkSeals")))
            {
                ShowFailMessageToClient("Please select seal.");
                return false;
            }
            else
            {
                string[] sealIds = QF("chkSeals").Trim().Split(',');

                foreach (string tmpId in sealIds)
                {
                    SealsEntity sealsEntity = list.Find(r => r.ID == int.Parse(tmpId));
                    if (sealsEntity == null)
                    {
                        ShowFailMessageToClient("Seal is void.");
                        return false;
                    }
                    else
                        sealRequestsEntity.SealList.Add(sealsEntity);
                }
            }

            if (app.SealRequestsUpdate(sealRequestsEntity))
            {
                return true;
            }
            return false;
        }

        private void ChangeRequestStatus_Saved()
        {
            SealRequestsEntity sealRequestsEntity = new SealRequestsEntity();

            if ((sealRequestsEntity = CheckData()) != null)
            {
                if (sealRequestsEntity.Status == RequestStatus.Draft && sealRequestsEntity.RequestedBy == UserInfo.UserID)
                {
                    if (SaveData(sealRequestsEntity))
                    {
                    }
                    else
                        ShowFailMessageToClient(app.BrokenRuleMessages);
                }
                else
                {
                    ShowFailMessageToClient("It is not editable.");
                    return;
                }
            }
        }

        private void UpdateRequestDetails()
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            sealRequestsEntity.Title = txtTitle.Text;
            sealRequestsEntity.Description = txtDescription.Text.Replace("\r\n", "<br>");
            app.SealRequestsUpdate(sealRequestsEntity);
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            int wfhisEntUpdateID = 0;
            WorkflowAction selectedAction = (WorkflowAction)Enum.Parse(typeof(WorkflowAction), ddlAction.SelectedValue);
            int nextUserID = HiddenFieldUsers.Value.Length > 0 ? int.Parse(HiddenFieldUsers.Value) : -1;
            sealRequestEntity = CheckData();

            // Update this record in WorkflowHistory table
            if ((ddlAction.SelectedValue == WorkflowAction.Submit.ToString() && sealRequestEntity.Status != RequestStatus.Denied ) || ddlAction.SelectedValue == WorkflowAction.Save.ToString())
            { }
            else
            {
                WorkflowHistoryEntity wfhisEntUpdate = new WorkflowHistoryEntity();
                wfhisEntUpdate.ProcessedBy = UserInfo.ID;
                wfhisEntUpdate.WorkflowRequestID = sealRequestEntity.ID;
                wfhisEntUpdate.ProcessedTime = DateTime.Now;
                wfhisEntUpdate.Action = selectedAction;
                wfhisEntUpdate.Comment = txtComments.Text;
                wfhisEntUpdateID = app.WorkflowHistoryUpdate(wfhisEntUpdate);
            }

            // Insert files
            if (divUploadFiles1.Visible)
            {
                if (fileupload1.PostedFile.ContentLength > 0 && IsValidFile(fileupload1.PostedFile.FileName))
                {
                    InsertFile(fileupload1.PostedFile, sealRequestEntity.ID, wfhisEntUpdateID, 1);
                }
                if (fileupload2.PostedFile.ContentLength > 0 && IsValidFile(fileupload2.PostedFile.FileName))
                {
                    InsertFile(fileupload2.PostedFile, sealRequestEntity.ID, wfhisEntUpdateID, 2);
                }
                if (fileupload3.PostedFile.ContentLength > 0 && IsValidFile(fileupload3.PostedFile.FileName))
                {
                    InsertFile(fileupload3.PostedFile, sealRequestEntity.ID, wfhisEntUpdateID, 3);
                }
            }
            if (divUploadFiles2.Visible)
            {
                if (fileupload4.PostedFile.ContentLength > 0 && IsValidFile(fileupload4.PostedFile.FileName))
                {
                    InsertFile(fileupload4.PostedFile, sealRequestEntity.ID, wfhisEntUpdateID, 1);
                }
                if (fileupload5.PostedFile.ContentLength > 0 && IsValidFile(fileupload5.PostedFile.FileName))
                {
                    InsertFile(fileupload5.PostedFile, sealRequestEntity.ID, wfhisEntUpdateID, 2);
                }
                if (fileupload6.PostedFile.ContentLength > 0 && IsValidFile(fileupload6.PostedFile.FileName))
                {
                    InsertFile(fileupload6.PostedFile, sealRequestEntity.ID, wfhisEntUpdateID, 3);
                }
            }


            // Update status in SealRequest table
            if (sealRequestEntity.Type == 0)
            {
                switch (selectedAction)
                {
                    case WorkflowAction.Submit:
                        if (sealRequestEntity.Status == RequestStatus.Denied && sealRequestEntity.RequestedBy == UserInfo.UserID)
                        {
                            SendSubmitEmail(sealRequestEntity);
                        }
                        else
                            ChangeRequestStatus_Submitted();
                        break;
                    case WorkflowAction.ForwardApproval:
                        ChangeRequestStatus_ForwardApproval();
                        break;
                    case WorkflowAction.Approve:
                        ChangeRequestStatus_Approved();
                        break;
                    case WorkflowAction.ForwardProcess:
                        ChangeRequestStatus_ForwardProcessing();
                        break;
                    case WorkflowAction.FinishProcess:
                        ChangeRequestStatus_Processed();
                        break;
                    case WorkflowAction.ContinueProcess:
                        ChangeRequestStatus(RequestStatus.PendingProcess);
                        break;
                    case WorkflowAction.Deny:
                        ChangeRequestStatus_Denied();
                        break;
                    case WorkflowAction.Complete:
                        ChangeRequestStatus_Completed();
                        break;
                    case WorkflowAction.Cancel:
                        ChangeRequestStatus_Canceled();
                        break;
                    case WorkflowAction.Save:
                        ChangeRequestStatus_Saved();
                        break;
                }
            }
            else
            {
                switch (selectedAction)
                {
                    case WorkflowAction.Submit:
                        ChangeRequestStatus(RequestStatus.Submitted);
                        UpdateRequestDetails();
                        SendEmailToRespUser(nextUserID);
                        break;
                    case WorkflowAction.ForwardApproval:
                        ChangeRequestStatus(RequestStatus.PendingApproval);
                        SendEmailToRespUser(nextUserID);
                        break;
                    case WorkflowAction.Approve:
                        ChangeRequestStatus(RequestStatus.Approved);
                        SendEmailToRespUser(nextUserID);
                        break;
                    case WorkflowAction.ForwardProcess:
                        ChangeRequestStatus(RequestStatus.PendingProcess);
                        SendEmailToRespUser(nextUserID);
                        break;
                    case WorkflowAction.FinishProcess:
                        ChangeRequestStatus(RequestStatus.Processed);
                        SendEmailToUser(true);
                        break;
                    case WorkflowAction.ContinueProcess:
                        ChangeRequestStatus(RequestStatus.PendingProcess);
                        SendEmailToRespUser(nextUserID);
                        break;
                    case WorkflowAction.Deny:
                        ChangeRequestStatus(RequestStatus.Denied);
                        SendEmailToUser(false);
                        break;
                    case WorkflowAction.Complete:
                        ChangeRequestStatus(RequestStatus.Completed);
                        break;
                    case WorkflowAction.Cancel:
                        ChangeRequestStatus(RequestStatus.Canceled);
                        break;
                    case WorkflowAction.Save:
                        UpdateRequestDetails();
                        break;
                }
            }

            // Insert next record in WorkflowHistory table

            //else if (sealRequestEntity.Type == 0 && (ddlAction.SelectedValue == "Submit" || ddlAction.SelectedValue == "Approve"))  //Possible multiple next responsible users
            //{
            //    string[] lstUserID = HiddenFieldUsers.Value.Split(',');
            //    foreach (string s in lstUserID)
            //    {
            //        InsertWorkflowHistory(int.Parse(s));
            //    }
            //}

            if (nextUserID != -1)
            {
                InsertWorkflowHistory(nextUserID);
            }

            // Redirect
            if (sealRequestEntity != null)
            {
                if (sealRequestEntity.Status == RequestStatus.Canceled)
                    Response.Redirect("SealRequests.aspx"); //取消申请后，跳转到列表页
                else
                    Redirect(string.Format("EditSealRequest.aspx?ID={0}&returnurl={1}"
                          , sealRequestEntity.ID, QS("returnurl")), true); //重定向一次
            }
            else
                ShowFailMessageToClient(app.BrokenRuleMessages);
        }

        private void ChangeRequestStatus(RequestStatus requestStatus)
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            if (sealRequestsEntity != null)
            {
                if (app.UpdateStatus(sealRequestsEntity.ID, requestStatus))
                { }
                else
                    ShowFailMessageToClient(app.BrokenRuleMessages);
            }
        }

        private void InsertWorkflowHistory(int userID)
        {
            WorkflowHistoryEntity wfhisEntInsert = new WorkflowHistoryEntity();
            wfhisEntInsert.WorkflowRequestID = sealRequestEntity.ID;
            wfhisEntInsert.CreatedTime = DateTime.Now;
            wfhisEntInsert.ProcessedBy = userID;
            wfhisEntInsert.Action = WorkflowAction.Pending;
            app.WorkflowHistoryInsert(wfhisEntInsert);
        }

        private void SendEmailToRespUser(int recipientID)
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            UsersEntity usersEntity = new App.UserApplication().GetUser(recipientID);

            string mailTemplatePath = Server.MapPath(@"~\Template\SendEmailToWorkFlowRespUser.txt");
            string mailTemplate = File.ReadAllText(mailTemplatePath);
            //string mailTitle = "[申请工作流审批] " + sealRequestsEntity.Title;

            string mailTitle = "";
            if (sealRequestEntity.Type == 0)
                mailTitle = "[Seal] " + sealRequestsEntity.Title;
            else if (sealRequestEntity.Type == 1)
            {
                mailTitle = "[WorkOrder] " + sealRequestsEntity.Title;
            }

            string content = mailTemplate.Replace("[ClientName]", usersEntity.FirstName).Replace("[applicant]", UserInfo.FirstName)
                     .Replace("[content]", sealRequestsEntity.Description);
            new SmtpClientEmailSender(new TextFileLogger()).SendMail(usersEntity.Email, Config.DefaultSendEmail, mailTitle, content);
        }

        private void SendEmailToUser(bool isSuccess)
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            string mailTemplatePath = Server.MapPath(@"~\Template\SendEmailToWorkFlowUser.txt");
            string mailTemplate = File.ReadAllText(mailTemplatePath);
            string isSuccessStr = isSuccess ? "Finished" : "Denied";
            string mailTitle = "[" + isSuccessStr + "] " + sealRequestsEntity.Title;

            UsersEntity usersEntity = new App.UserApplication().GetUser(sealRequestsEntity.RequestedBy);

            string content = mailTemplate.Replace("[ClientName]", usersEntity.FirstName)
                    .Replace("[content]", sealRequestsEntity.Description)
                    .Replace("[isSuccess]", isSuccessStr.ToLower());
            new SmtpClientEmailSender(new TextFileLogger()).SendMail(usersEntity.Email, Config.DefaultSendEmail, mailTitle, content);
        }

        protected void rptWorkflowHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Bind files
                Repeater rep = e.Item.FindControl("rptWorkflowHistoryFiles") as Repeater;
                rep.DataSource = (List<SealFileEntity>)DataBinder.Eval(e.Item.DataItem, "lstFiles");
                rep.DataBind();


                // Change color of column "Action"
                Label lbl = e.Item.FindControl("lblAction") as Label;

                if (lbl.Text == "Pending")
                {
                    lbl.ForeColor = Color.Red;
                }

                // Eliminate minvalue of column "ProcessedTime"
                lbl = e.Item.FindControl("lblProcessedTime") as Label;
                if (lbl.Text == DateTime.MinValue.ToString())
                {
                    lbl.Text = "";
                }
            }
        }
    }
}