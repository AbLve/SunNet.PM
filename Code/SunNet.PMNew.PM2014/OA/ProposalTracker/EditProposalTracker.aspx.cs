using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using System.Text;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.FileModel;
using StructureMap;
using System.IO;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Entity.ProposalTrackerModel.Enums;

namespace SunNet.PMNew.PM2014.OA.ProposalTracker
{
    public partial class EditProposalTracker : BasePage
    {
        UserApplication userApp = new UserApplication();
        ProposalTrackerApplication wrApp = new ProposalTrackerApplication();
        FileApplication fileApp = new FileApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserInfo.Role != RolesEnum.ADMIN && UserInfo.Role != RolesEnum.PM && UserInfo.Role != RolesEnum.Sales)
            {
                ShowFailMessageToClient("unauthorized access.");
                return;
            }
            if (QS("ID", 0) == 0)
            {
                ShowFailMessageToClient("unauthorized access.");
                return;
            }

            if (!IsPostBack)
            {
                ProposalTrackerEntity entity = new App.ProposalTrackerApplication().Get(QS("ID", 0));
                if (entity == null)
                {
                    ShowFailMessageToClient("unauthorized access.");
                    return;
                }
                InitProject(entity);
                BindStatus(entity.Status);
                BindData(entity);
                CheckStatus(entity.Status);
                this.ddlStatus.Attributes.Add("onchange", "statusChange()");
            }
        }



        private void InitProject(ProposalTrackerEntity entity)
        {
            ProjectApplication projApp = new ProjectApplication();

            List<ProjectDetailDTO> list = projApp.GetUserProjectsForCreateObject(UserInfo, entity.ProjectID);

            if (list.Count > 1 || list.Count == 0)
            {
                list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "Please select...", "0");
            }
            else if (list.Count == 1)
            {
                list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID");
            }
        }

        private void BindStatus(int status)
        {
            if (status == 7)
            {
                ddlStatus.Items.Add(new ListItem() { Text = "Awaiting ETA", Value = "1" });
                ddlStatus.Items.Add(new ListItem() { Text = "Awaiting Proposal", Value = "2" });
                ddlStatus.Items.Add(new ListItem() { Text = "Awaiting Approval/PO", Value = "3" });
                ddlStatus.Items.Add(new ListItem() { Text = "Awaiting Development", Value = "4" });
                ddlStatus.Items.Add(new ListItem() { Text = "Awaiting Sending Invoice", Value = "5" });
                ddlStatus.Items.Add(new ListItem() { Text = "Awaiting Payment", Value = "6" });
                ddlStatus.Items.Add(new ListItem() { Text = "Paid/Completed", Value = "7" });
                ddlStatus.Items.Add(new ListItem() { Text = "On Hold", Value = "8" });
                ddlStatus.Items.Add(new ListItem() { Text = "Not Approved", Value = "9" });
            }
            else
            {
                ddlStatus.Items.Add(new ListItem() { Text = "Awaiting ETA", Value = "1" });
                ddlStatus.Items.Add(new ListItem() { Text = "Awaiting Proposal", Value = "2" });
                ddlStatus.Items.Add(new ListItem() { Text = "Awaiting Approval/PO", Value = "3" });
                ddlStatus.Items.Add(new ListItem() { Text = "Awaiting Development", Value = "4" });
                ddlStatus.Items.Add(new ListItem() { Text = "Awaiting Sending Invoice", Value = "5" });
                ddlStatus.Items.Add(new ListItem() { Text = "Awaiting Payment", Value = "6" });
                ddlStatus.Items.Add(new ListItem() { Text = "On Hold", Value = "8" });
                ddlStatus.Items.Add(new ListItem() { Text = "Not Approved", Value = "9" });
            }
        }
        private void BindData(ProposalTrackerEntity entity)
        {
            ListItem li = ddlProject.Items.FindByValue(entity.ProjectID.ToString());
            if (li == null)
            {
                ProjectsEntity projectEntity = new App.ProjectApplication().Get(entity.ProjectID);
                if (projectEntity != null)
                    ddlProject.Items.Insert(0, new ListItem() { Text = projectEntity.Title, Value = projectEntity.ID.ToString() });
            }
            else
                li.Selected = true;

            //ddlPayment.SelectedValue = entity.Payment.ToString();
            //txtRequestNo.Text = entity.RequestNo;
            //txtInvoiceNo.Text = entity.InvoiceNo;
            ddlStatus.SelectedValue = entity.Status.ToString();
            //txtDueDate.Text = entity.DueDate == null ? "" : ((DateTime)entity.DueDate).ToString("MM/dd/yyyy");
            txtTitle.Text = entity.Title;
            txtDescription.Text = entity.Description;

            txtProposalSentTo.Text = entity.ProposalSentTo;
            txtProposalSentOn.Text = entity.ProposalSentOn == null ? "" : entity.ProposalSentOn.Value.ToShortDateString();
            txtPO.Text = entity.PONo;
            chkLessThen.Checked = entity.PoTotalLessThenProposalTotal.Value;
            txtApprovedBy.Text = entity.ApprovedBy;
            txtApprovedOn.Text = entity.ApprovedOn == null ? "" : entity.ApprovedOn.Value.ToShortDateString();
            //txtInvoiceSentOn.Text = entity.InvoiceSentOn == null ? "" : entity.InvoiceSentOn.Value.ToShortDateString();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<ul class=\"fileList\"><li><a href=\"/do/DoDownWrokRequestFile.ashx?ID={0}\" title='{1}' target=\"_blank\">{1}</a>"
                , entity.ProposalTrackerID, entity.WorkScopeDisplayName);
            sb.Append("</li></ul>");
            lblFile.InnerHtml = sb.ToString();


            rptNotes.DataSource = wrApp.GetProposalTrackerNotes(entity.ProposalTrackerID); ;
            rptNotes.DataBind();

            rptDocuments.DataSource = fileApp.GetFileListBySourceId(entity.ProposalTrackerID, FileSourceType.WorkRequest);
            rptDocuments.DataBind();

            decimal hours = wrApp.GetProposalTrackerHours(QS("id", 0));

            string link = string.Format("<a href='/Report/Timesheet.aspx?WID={0}'>{1}</a>",
                entity.ProposalTrackerID, hours.ToString("N"));

            litHours.Text = link;
            litProjectName.Text = ddlProject.SelectedItem.Text;

            this.rptTicketsList.DataSource = wrApp.GetAllRelation(entity.ProposalTrackerID);
            this.rptTicketsList.DataBind();
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool result = true;
            if (fileUpload.HasFile)
            {
                string fileContentType = fileUpload.PostedFile.ContentType;
                if ((fileContentType != "application/msword" && fileContentType != "application/pdf"
                && fileContentType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
                {
                    ShowFailMessageToClient("Please select a file to upload ( *.doc, *.docx, *.pdf)");
                    return;
                }
            }

            ProposalTrackerEntity entity = new App.ProposalTrackerApplication().Get(QS("ID", 0));

            // basic infomation
            entity.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
            //entity.Payment = Convert.ToInt32(this.ddlPayment.SelectedValue);
            entity.InvoiceNo = "";
            bool sendEmailToTeam = entity.Status != Convert.ToInt32(ddlStatus.SelectedValue);
            string oldStatusName = ((ProposalTrackerStatusEnum)entity.Status).ToString().Replace("_", " ");
            entity.Status = Convert.ToInt32(ddlStatus.SelectedValue);
            string newStatusName = ((ProposalTrackerStatusEnum)entity.Status).ToString().Replace("_", " ");

            entity.Title = txtTitle.Text.Trim();
            entity.Description = txtDescription.Text.Trim();

            entity.ModifyOn = DateTime.Now;
            entity.ModifyBy = UserInfo.UserID;


            entity.ProposalSentTo = txtProposalSentTo.Text.Trim();
            DateTime proposalSentOn;
            if (DateTime.TryParse(txtProposalSentOn.Text.Trim(), out proposalSentOn))
            {
                entity.ProposalSentOn = proposalSentOn;
            }
            else
            {
                entity.ProposalSentOn = null;
            }
            entity.PoTotalLessThenProposalTotal = chkLessThen.Checked;
            entity.PONo = txtPO.Text.Trim();
            entity.ApprovedBy = txtApprovedBy.Text.Trim();
            DateTime approvedOn;
            if (DateTime.TryParse(txtApprovedOn.Text.Trim(), out approvedOn))
            {
                entity.ApprovedOn = approvedOn;
            }
            else
            {
                entity.ApprovedOn = null;
            }
            //DateTime invoiceSentOn;
            //if (DateTime.TryParse(txtInvoiceSentOn.Text.Trim(), out invoiceSentOn))
            //{
            //    entity.InvoiceSentOn = invoiceSentOn;
            //}
            //else
            //{
            //    entity.InvoiceSentOn = null;
            //}


            if (fileUpload.HasFile)
            {
                string fileContentType = fileUpload.PostedFile.ContentType;
                entity.WorkScope = UtilFactory.Helpers.FileHelper.SaveUploadFiles("WorkScope", entity.ProjectID, fileUpload.PostedFile);
                entity.WorkScopeDisplayName = fileUpload.FileName;

                if (wrApp.UpdateProposalTracker(entity))
                {
                    if (sendEmailToTeam)
                    {
                        string subject = "[ " + entity.Title + " ]" + " -- " + "[ " + newStatusName + " ]";
                        string content = UserInfo.FirstAndLastName + " changed [" + entity.Title + "]'s status from [" + oldStatusName + "] to " + "[" + newStatusName + "] on " + entity.ModifyOn;
                        ObjectFactory.GetInstance<IEmailSender>().SendMail("team@sunnet.us", Config.DefaultSendEmail, subject, content);
                    }
                    FilesEntity model = FileFactory.CreateFileEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                    model.FilePath = entity.WorkScope;
                    model.ProposalTrackerId = entity.ProposalTrackerID;
                    model.ContentType = fileUpload.PostedFile.ContentType;
                    model.FileID = 0;
                    model.FileSize = fileUpload.PostedFile.ContentLength;
                    model.FileTitle = Path.GetFileName(fileUpload.FileName);
                    model.IsDelete = false;
                    model.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
                    model.SourceType = (int)FileSourceType.WorkRequestScope;
                    model.ThumbPath = Path.GetFileName(fileUpload.PostedFile.FileName);
                    result = fileApp.AddFile(model) > 0 ? true : false;
                    if (string.IsNullOrEmpty(QS("returnurl")))
                        Redirect("index.aspx");
                    else
                        Redirect(QS("returnurl"));
                    //Redirect(string.Format("EditProposalTracker.aspx?ID={0}&returnurl={1}", entity.ProposalTrackerID, QS("returnurl")));
                }
                else
                {
                    ShowFailMessageToClient(wrApp.BrokenRuleMessages);
                    return;
                }
            }
            else
            {
                if (wrApp.UpdateProposalTracker(entity))
                {
                    if (sendEmailToTeam)
                    {
                        string subject = "[ " + entity.Title + " ]" + " -- " + "[ " + newStatusName + " ]";
                        string content = UserInfo.FirstAndLastName + " changed [" + entity.Title + "]'s status from [" + oldStatusName + "] to " + "" + newStatusName + "] on " + entity.ModifyOn;
                        ObjectFactory.GetInstance<IEmailSender>().SendMail("team@sunnet.us", Config.DefaultSendEmail, subject, content);
                    }
                    Redirect(QS("returnurl"));
                    //Redirect(string.Format("EditProposalTracker.aspx?ID={0}&returnurl={1}", entity.ProposalTrackerID, QS("returnurl")));
                }
                else
                {
                    ShowFailMessageToClient(wrApp.BrokenRuleMessages);
                    return;
                }
            }
        }

        protected string ShowPriorityImgByDevDate(string date)
        {
            string imgString = "";

            DateTime Now = DateTime.Now.Date;

            TimeSpan Diff = Convert.ToDateTime(date).Subtract(Now);

            int DiffDate = Diff.Days;

            if (DiffDate <= 3 && DiffDate > 0)
            {
                imgString = "<img src='/icons/orange.gif' />";
            }
            else if (DiffDate == 0)
            {
                imgString = "<img src='/icons/yellow.gif' />";
            }
            else if (Convert.ToDateTime(date) > UtilFactory.Helpers.CommonHelper.GetDefaultMinDate() && DiffDate < 0)
            {
                imgString = "<img src='/icons/red.gif' />";
            }

            return imgString;
        }

        protected void rpt_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lid = e.Item.FindControl("ltlCreatedByID") as Literal;
                Literal lname = e.Item.FindControl("ltlCreatedByName") as Literal;
                lname.Text = userApp.GetUser(Convert.ToInt32(lid.Text)).FirstName;
            }
        }

        protected void CheckStatus(int status)
        {
            if (status == 7 || status == 9)
            {
                this.ddlProject.Enabled = false;
                this.ddlStatus.Enabled = false;
                this.txtTitle.Enabled = false;
                this.txtDescription.Enabled = false;
                this.fileUpload.Enabled = false;
                this.txtProposalSentTo.Enabled = false;
                this.txtProposalSentOn.Enabled = false;
                this.txtPO.Enabled = false;
                this.txtApprovedBy.Enabled = false;
                this.txtApprovedOn.Enabled = false;
                //this.txtInvoiceNo.Enabled = false;
                //this.txtInvoiceSentOn.Enabled = false;
                this.btnSave.Enabled = false;
            }
        }
    }
}