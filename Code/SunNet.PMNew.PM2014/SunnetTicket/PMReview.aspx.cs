using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Impl.SqlDataProvider.Ticket;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils.Providers;
using System.IO;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.PM2014.Ticket.Sunnet
{
    public partial class PMReview : BasePage
    {
        TicketsApplication ticketApp = new TicketsApplication();
        ProjectApplication projApp = new ProjectApplication();
        UserApplication userApp = new UserApplication();
        protected static TicketsEntity ticketsEntity;
        FeedBackApplication fbAPP = new FeedBackApplication();
        FileApplication fileApp = new FileApplication();
        ProposalTrackerApplication wrApp = new ProposalTrackerApplication();

        protected int ticketID { get; set; }
        protected int projectID { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            ((Pop)(this.Master)).Width = 770;
            ticketID = QS("tid", 0);
            projectID = QS("pid", 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ticketID > 0)
                {
                    ticketsEntity = ticketApp.GetTickets(ticketID);

                    //创建者
                    TicketUsersEntity ticketUser = ticketApp.GetTicketCreateUser(ticketID);
                    UsersEntity createUser = userApp.GetUser(ticketUser.UserID);
                    if (createUser.Role == RolesEnum.CLIENT)
                    {
                        HiddenField_TicketCreateId.Value = "-1";
                    }
                    else
                    {
                        HiddenField_TicketCreateId.Value = ticketUser.UserID.ToString(); ;
                    }


                    ProposalTrackerRelationEntity model = wrApp.GetProposalTrackerByTid(ticketID);
                    if (model != null && model.ID != 0)
                    {
                        ddl_Proposal.SelectedValue = model.WID.ToString();
                    }
                    ddlResponsibleUser.Items.Add(new ListItem("System", "-1"));
                }
                if (projectID > 0)
                {
                    ProposalTrackerApplication wrApp = new ProposalTrackerApplication();

                    List<ProposalTrackerEntity> proposalTracker = wrApp.GetProposalTrackerByPid(projectID);
                    ddl_Proposal.DataSource = proposalTracker;
                    ddl_Proposal.DataValueField = "ProposalTrackerID";
                    ddl_Proposal.DataTextField = "Title";
                    ddl_Proposal.DataBind();
                    ddl_Proposal.Items.Insert(0, new ListItem("Please select... ", ""));
                }
            }
            if (ticketsEntity != null)
            {
                if (!IsPostBack)
                {
                    if (ticketsEntity.Status >= TicketsState.PM_Reviewed &&
                        ticketsEntity.Status <= TicketsState.Estimation_Approved && ticketsEntity.TicketType == TicketsType.Bug)
                    {
                        dvChangeRequest.Style.Add("display", "block");
                    }
                    else
                    {
                        dvChangeRequest.Style.Add("display", "none");
                    }
                    litHead.Text = string.Format("Ticket ID: {0}, {1}&nbsp;&nbsp; ; &nbsp;&nbsp;Status: {2}"
                        , ticketsEntity.TicketID, ticketsEntity.Title, ticketsEntity.Status.ToText());
                    TicketUsersEntity ticketUsersEntity = ticketApp.GetTicketCreateUser(ticketsEntity.TicketID);
                    SetEstimation(ticketUsersEntity);
                    SetEditClient(ticketUsersEntity);
                    BindStatus();

                    txtProprosalName.Text = ticketsEntity.ProprosalName;
                    txtWorkPlanName.Text = ticketsEntity.WorkPlanName;
                    txtWorkScope.Text = ticketsEntity.WorkScope;
                    txtInvoice.Text = ticketsEntity.Invoice;
                }
            }
            else
            {
                ShowFailMessageToClient();
            }
        }

        private void SetEditClient(TicketUsersEntity ticketUsersEntity)
        {
            if (ticketsEntity.IsInternal == true)
            {
                dvClient.Visible = false;
            }
            else
            {
                List<UsersEntity> clientUsers = projApp.GetPojectClientUsers(ticketsEntity.ProjectID,
                    projApp.Get(ticketsEntity.ProjectID).CompanyID);

                ddlClient.DataSource = clientUsers;

                ddlClient.DataBind();

                clientUsers.BindDropdown(ddlClient, UserNameDisplayProp, "UserID", "Please select...", "-1");
                if (null == ticketUsersEntity || userApp.GetUser(ticketUsersEntity.UserID).Role != RolesEnum.CLIENT)
                {
                    ddlClient.SelectedValue = "-1";
                }
                else
                {
                    ddlClient.SelectedValue = ticketUsersEntity.UserID.ToString();
                }
                //找一下ticket的createuser是不是Client
                //如果是则选中这个client 
                //否则选择please select
                dvClient.Visible = true;
            }
        }

        private void BindStatus()
        {
            switch (ticketsEntity.Status)
            {
                case TicketsState.Not_Approved:
                    {
                        ddlStatus.SelectedValue = ((int)TicketsState.PM_Reviewed).ToString();
                        break;
                    }
                case TicketsState.Submitted:
                    {
                        ddlStatus.SelectedValue = ((int)TicketsState.PM_Reviewed).ToString();
                        break;
                    }
                case TicketsState.Waiting_For_Estimation:
                case TicketsState.PM_Verify_Estimation:
                    {
                        ddlStatus.SelectedValue = ((int)TicketsState.Waiting_Confirm).ToString();
                        break;
                    }
                case TicketsState.Waiting_Confirm:
                    ddlStatus.SelectedValue = "-1";
                    //ddlStatus.SelectedValue= ((int)TicketsState.Estimation_Approved).ToString();
                    break;
                case TicketsState.Tested_Success_On_Client:
                    {
                        if (ticketsEntity.IsInternal)
                        {
                            ddlStatus.SelectedValue = ((int)TicketsState.Completed).ToString();
                        }
                        else
                        {
                            ddlStatus.SelectedValue = ((int)TicketsState.Ready_For_Review).ToString();
                        }
                        break;
                    }
                case TicketsState.PM_Reviewed:
                    {
                        if (ticketsEntity.IsEstimates)
                        {
                            ddlStatus.SelectedValue = ((int)TicketsState.PM_Reviewed).ToString();
                        }
                        else
                            ddlStatus.SelectedValue = ((int)TicketsState.Developing).ToString();
                        break;
                    }
                default: ddlStatus.SelectedValue = "-1"; break;

            }
        }

        public void SetEstimation(TicketUsersEntity ticketUsersEntity)
        {
            if ((int)ticketsEntity.Status > (int)TicketsState.PM_Reviewed && ticketsEntity.EsUserID > 0)
            {
                ltrlEstimationUser.Text = userApp.GetUser(ticketsEntity.EsUserID).FirstAndLastName;
            }
            else
            {
                ltrlEstimationUser.Text = UserInfo.FirstAndLastName;
            }

            List<UsersEntity> listUsers = userApp.GetActiveUserList().FindAll(u => u.UserType == "CLIENT" && u.CompanyID == ticketsEntity.CompanyID)
                .OrderBy(r => r.FirstName).ToList();

            foreach (UsersEntity u in listUsers)
            {
                ddlConfirmEstmateUserId.Items.Add(new ListItem() { Value = u.UserID.ToString(), Text = string.Format("{0}({1})", u.FirstAndLastName, u.Role) });
            }

            if (ticketsEntity.ConfirmEstmateUserId > 0)
            {
                ListItem li = ddlConfirmEstmateUserId.Items.FindByValue(ticketsEntity.ConfirmEstmateUserId.ToString());
                if (li != null)
                    li.Selected = true;
            }

            txtInitialTime.Text = ticketsEntity.InitialTime.ToString("N");
            txtBoxExtimationHours.Text = ticketsEntity.FinalTime.ToString("N");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int tmpState = int.Parse(ddlStatus.SelectedValue);
            TicketsState selectState = (TicketsState)tmpState;
            if (rdoAccounting.SelectedValue == "1")
            {
                //int wid = int.Parse(ddl_Proposal.SelectedValue);
                int wid = int.Parse(hid_Proposal.Value);
                ProposalTrackerRelationEntity model = wrApp.GetProposalTrackerByTid(ticketID);
                if (model == null || model.ID == 0)
                {
                    model.CreatedBy = IdentityContext.UserID;
                    model.TID = ticketID;
                    model.WID = wid;
                    wrApp.AddProposalTrackerRelation(model);
                }
                else
                {
                    if (model.TID != ticketID || model.WID != wid)
                    {
                        model.TID = ticketID;
                        model.WID = wid;
                        wrApp.UpdateProposalByProposal(model);
                    }
                }
            }
            else
            {
                ProposalTrackerRelationEntity model = wrApp.GetProposalTrackerByTid(ticketID);
                if (model != null || model.ID > 0)
                {
                    wrApp.DelProposalTrackerRelationByID(model.ID);
                }

            }

            bool result = false;
            //指定初次估时者
            int estimationUserID = 0;

            bool isEstimation = rdoEstimationYes.Checked;
            if (isEstimation && selectState == TicketsState.PM_Reviewed)
            {
                if (!int.TryParse(QF(ddlEstUser.UniqueID), out estimationUserID))
                {
                    ShowFailMessageToClient("Invalid extimation user.");
                    return;
                }
            }

            //选择accounting

            string accounting = (rdoAccounting.SelectedValue == "" ? "0" : rdoAccounting.SelectedValue);
            ticketsEntity.Accounting = (AccountingState)int.Parse(accounting);


            //指派最终确认估时者
            int confirmEstmateUserId = 0;
            if (selectState == TicketsState.Waiting_Confirm)
            {
                if (!int.TryParse(QF(ddlConfirmEstmateUserId.UniqueID), out confirmEstmateUserId))
                {
                    ShowFailMessageToClient("Invalid Waiting Confirm user.");
                    return;
                }
            }

            decimal initialtionHours;
            //初次估时
            if (!decimal.TryParse(txtInitialTime.Text, out initialtionHours))
            {
                ShowFailMessageToClient("Invalid extimation hour.");
                return;
            }

            decimal extimationHours;
            ///最终估时
            if (!decimal.TryParse(txtBoxExtimationHours.Text, out extimationHours))
            {
                ShowFailMessageToClient("Invalid extimation hour.");
                return;
            }


            ///没有达到预期要求
            if (((TicketsState)int.Parse(ddlStatus.SelectedValue)) == TicketsState.PM_Deny)
            {
                HttpFileCollection files = Request.Files;
                string fileuploadErrMsg = string.Empty;
                for (int i = 0; i < files.Count; i++)
                {

                    if (files[i].ContentLength > 0 || txtBoxDenyReason.Text.Trim().Length > 0)
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
                            if (!InsertFile(files[i], ticketsEntity.ProjectID, false,
                                    feedbacksEntity.ID))
                            {
                                fileuploadErrMsg += files[i].FileName + " Upload failed.";
                                result = false;
                            }
                        }
                    }
                }
            }

            if (selectState == TicketsState.Estimation_Approved)
            {
                ticketsEntity.ProprosalName = txtProprosalName.Text;
                ticketsEntity.WorkPlanName = txtWorkPlanName.Text;
                ticketsEntity.WorkScope = txtWorkScope.Text;
                ticketsEntity.Invoice = txtInvoice.Text;
            }



            int responsibleUserId;
            int.TryParse(QF(ddlResponsibleUser.UniqueID), out responsibleUserId);
            int oldResponsibleUserId = ticketsEntity.ResponsibleUser;
            ticketsEntity.ResponsibleUser = responsibleUserId;

            ProjectsEntity projectEntity = projApp.Get(ticketsEntity.ProjectID);

            int newClientId = -1;
            if (ddlClient.Visible == true)
            {
                if (ddlClient.SelectedValue != "-1")
                {
                    newClientId = int.Parse(ddlClient.SelectedValue);
                }
            }   

            result = ticketApp.PMReview(ticketsEntity, UserInfo, rdoRequestYes.Checked,
                ddlStatus.SelectedValue, isEstimation, estimationUserID, confirmEstmateUserId, initialtionHours, extimationHours
               , ticketUsersView.GetSelectedUserList(), txtBoxConvertDescr.Text,
              txtBoxDenyReason.Text.Trim(), projectEntity.PMID, newClientId);

            if (ddlClient.Visible == true)
            {
                if (ddlClient.SelectedValue != "-1")
                {
                    ticketApp.UpdateCreateUser(int.Parse(ddlClient.SelectedValue), ticketsEntity.TicketID);
                }
            }

            if (result)
            {
                //sent email to responsible user 2017/10/23
                if (oldResponsibleUserId != ticketsEntity.ResponsibleUser)
                {
                    ticketApp.SendEmailToResponsibile(ticketsEntity, UserInfo);
                }
                // 更新所有相关人员的WorkingOn状态
                ticketApp.UpdateWorkingOnStatus(ticketID, selectState);
                Redirect(EmptyPopPageUrl, false, true);
            }
            else
            {
                ShowFailMessageToClient();
            }
        }

        private FeedBacksEntity GetFeedbacksEntity()
        {
            var feedbacksEntity = new FeedBacksEntity
            {
                IsDelete = false,
                TicketID = ticketID,
                Title = string.Empty,
                Description = txtBoxDenyReason.Text.Trim(),
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
            fileEntity.TicketId = ticketID;
            fileEntity.ProjectId = projectID;
            fileEntity.SourceType = (int)FileSourceType.FeedBack;
            fileEntity.ThumbPath = "";
            fileEntity.CreatedOn = DateTime.Now.Date;
            fileEntity.CompanyID = ticketApp.GetCompanyIdByTicketId(ticketID);
            return fileApp.AddFile(fileEntity) > 0;
        }
    }
}