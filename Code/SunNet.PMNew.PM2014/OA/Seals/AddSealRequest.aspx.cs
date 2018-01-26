using System;
using System.Collections.Generic;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Entity.SealModel.Enum;
using System.Web.UI.WebControls;
using System.IO;
using SunNet.PMNew.Framework.Utils.Providers;
using StructureMap;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.PM2014.OA.Seals
{
    public partial class AddSealRequest : BasePage
    {
        SealsApplication app = new SealsApplication();
        UserApplication userApp = new UserApplication();

        private static SealRequestsEntity sealRequestEntity;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (UserInfo.Office == "CN" && UserInfo.Role != RolesEnum.PM) return;
                BindDropDownData();
                btnSave.Visible = true;
                BindDdlAction();
            }
        }
        private void BindDdlAction()
        {
            ddlAction.Items.Add(new ListItem() { Text = "Please select action", Value = "-1" });
            ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.Save.WorkflowActionToText(), Value = WorkflowAction.Save.ToString() });
            ddlAction.Items.Add(new ListItem() { Text = WorkflowAction.Submit.WorkflowActionToText(), Value = WorkflowAction.Submit.ToString() });
        }
        private void BindDropDownData()
        {
            List<SealsEntity> list = app.GetList().FindAll(r => r.Status == Status.Active);
            rptSeals.DataSource = list;
            rptSeals.DataBind();

            //SearchUsersRequest requestUser = new SearchUsersRequest(SearchUsersType.All, false, " FirstName ", " ASC ");
            //requestUser.IsSunnet = true;
            //SearchUserResponse responseuser = userApp.SearchUsers(requestUser);
            //ddlNextApprover.DataSource = responseuser.ResultList.FindAll(r => r.UserID != UserInfo.UserID);
            //ddlNextApprover.DataBind();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SealRequestsEntity sealRequestsEntity = new SealRequestsEntity();

            List<SealsEntity> list = app.GetList().FindAll(r => r.Status == Status.Active);
            sealRequestsEntity.Title = txtTitle.Text;
            sealRequestsEntity.Description = txtDescription.Text.Replace("\r\n", "<br>");
            sealRequestsEntity.RequestedBy = UserInfo.UserID;
            sealRequestsEntity.RequestedDate = DateTime.Now;
            sealRequestsEntity.Status = RequestStatus.Draft;
            sealRequestsEntity.Type = ddlType.SelectedValue == "Seal" ? 0 : 1;

            sealRequestsEntity.SealList = new List<SealsEntity>();
            if (ddlType.SelectedValue == "Seal")
            {
                if (string.IsNullOrEmpty(QF("chkSeals")))
                {
                    ShowFailMessageToClient("Please select seal.");
                    return;
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
                            break;
                        }
                        else
                            sealRequestsEntity.SealList.Add(sealsEntity);
                    }
                }
            }
            var id = app.SealRequestsInsert(sealRequestsEntity);
            if ((sealRequestsEntity.ID = id) > 0)
            {
                //// Insert Work flow history record: Save
                //WorkflowHistoryEntity wfhisEntity = new WorkflowHistoryEntity();
                //wfhisEntity.WorkflowRequestID = sealRequestsEntity.ID;
                //wfhisEntity.CreatedTime = DateTime.Now;
                //wfhisEntity.Action = WorkflowAction.Save;
                //wfhisEntity.ProcessedTime = DateTime.Now;
                //wfhisEntity.ProcessedBy = UserInfo.ID;
                //wfhisEntity.Comment = txtDescription.Text;
                //int wfhisID = app.WorkflowHistoryInsertFirst(wfhisEntity);

                //// Insert Work flow history record of next approver
                //wfhisEntity = new WorkflowHistoryEntity();
                //wfhisEntity.WorkflowRequestID = sealRequestsEntity.ID;
                //wfhisEntity.CreatedTime = DateTime.Now;
                //wfhisEntity.ProcessedBy = UserInfo.ID;
                //wfhisEntity.Action = WorkflowAction.Pending;
                //app.WorkflowHistoryInsert(wfhisEntity);

                // Insert Files
                if (fileupload1.PostedFile.ContentLength > 0 && IsValidFile(fileupload1.PostedFile.FileName))
                {
                    InsertFile(fileupload1.PostedFile, sealRequestsEntity.ID, 0, 1);
                }
                if (fileupload2.PostedFile.ContentLength > 0 && IsValidFile(fileupload2.PostedFile.FileName))
                {
                    InsertFile(fileupload2.PostedFile, sealRequestsEntity.ID, 0, 2);
                }
                if (fileupload3.PostedFile.ContentLength > 0 && IsValidFile(fileupload3.PostedFile.FileName))
                {
                    InsertFile(fileupload3.PostedFile, sealRequestsEntity.ID, 0, 3);
                }
                //合并步骤
                if (id > 0)
                {
                    SecondSave(id);
                }
            }
            else
            {
                ShowFailMessageToClient(app.BrokenRuleMessages);
                return;
            }
            //if (ddlType.SelectedValue == "Seal")
            //{
            //    string[] sealIds1 = QF("chkSeals").Trim().Split(',');
            //    foreach (string sealID in sealIds1)
            //    {
            //        wfhisEntity.ProcessedBy = int.Parse(sealID);
            //        app.WorkflowHistoryInsert(wfhisEntity);
            //    }
            //}
            //else
            //{
            //    wfhisEntity.ProcessedBy = int.Parse(ddlNextApprover.SelectedValue);
            //    app.WorkflowHistoryInsert(wfhisEntity);
            //}
            Redirect("SealRequests.aspx");
        }

        private void SecondSave(int id)
        {
            int nextUserID = HiddenFieldUsers.Value.Length > 0 ? int.Parse(HiddenFieldUsers.Value) : -1;
            sealRequestEntity = app.GetSealRequests(id);
            WorkflowAction selectedAction = (WorkflowAction)Enum.Parse(typeof(WorkflowAction), ddlAction.SelectedValue);
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
                    case WorkflowAction.Save:
                        UpdateRequestDetails();
                        break;
                }
            }
            if (nextUserID != -1)
            {
                InsertWorkflowHistory(nextUserID);
            }
        }

        private void InsertWorkflowHistory(int RequestID, int userID, WorkflowAction action)
        {
            WorkflowHistoryEntity wfhisEntInsert = new WorkflowHistoryEntity();
            wfhisEntInsert.WorkflowRequestID = RequestID;
            wfhisEntInsert.CreatedTime = DateTime.Now;
            wfhisEntInsert.ProcessedBy = userID;
            wfhisEntInsert.Action = action;
            app.WorkflowHistoryInsert(wfhisEntInsert);
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

        /// <summary>
        /// Submit
        /// </summary>
        private void ChangeRequestStatus_Submitted()
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            if (sealRequestsEntity != null)
            {
                if ((sealRequestsEntity.Status != RequestStatus.Draft && sealRequestsEntity.Status != RequestStatus.Denied) || sealRequestsEntity.RequestedBy != UserInfo.UserID)
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

        private SealRequestsEntity CheckData()
        {
            return sealRequestEntity;

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
        private void UpdateRequestDetails()
        {
            SealRequestsEntity sealRequestsEntity = CheckData();
            sealRequestsEntity.Title = txtTitle.Text;
            sealRequestsEntity.Description = txtDescription.Text.Replace("\r\n", "<br>");
            app.SealRequestsUpdate(sealRequestsEntity);
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
    }
}