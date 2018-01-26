using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.FileModel;
using StructureMap;
using System.IO;

namespace SunNet.PMNew.PM2014.OA.ProposalTracker
{
    public partial class AddProposalTracker : BasePage
    {
        ProposalTrackerApplication wrApp = new ProposalTrackerApplication();
        FileApplication fileApp = new FileApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitProject();
                this.ddlStatus.Attributes.Add("onchange", "statusChange()");
            }
        }

        private void InitProject()
        {
            ProjectApplication projApp = new ProjectApplication();

            List<ProjectDetailDTO> list = projApp.GetUserProjectsForCreateObject(UserInfo);

            if (list.Count > 1 || list.Count == 0)
            {
                list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "Please select...", "0");
            }
            else if (list.Count == 1)
            {
                list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID");
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool result = false;
            ProposalTrackerEntity entity = GetEntity();

            //Check Work Scope Format First  David02/25/2016
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

            int id = wrApp.AddProposalTracker(entity);
            if (id > 0)
            {
                if (fileUpload.HasFile) //Save Work Scope to 'Fiels' Table
                {
                    FilesEntity model = FileFactory.CreateFileEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                    model.FilePath = entity.WorkScope;

                    model.ProposalTrackerId = id;
                    model.ContentType = fileUpload.PostedFile.ContentType;
                    model.FileID = 0;
                    model.FileSize = fileUpload.PostedFile.ContentLength;
                    model.FileTitle = Path.GetFileName(fileUpload.FileName);
                    model.IsDelete = false;
                    model.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
                    model.SourceType = (int)FileSourceType.WorkRequestScope;
                    model.ThumbPath = Path.GetFileName(fileUpload.PostedFile.FileName);
                    result = fileApp.AddFile(model) > 0 ? true : false;
                }

                Redirect(entity.Status < 4 ? "index.aspx" : "EditProposalTracker.aspx?ID=" + id + "&returnurl=/OA/ProposalTracker/Index.aspx");
            }
            else
            {
                ShowFailMessageToClient(wrApp.BrokenRuleMessages);
            }

            /* David 0225 Changed the work scope is not required
            if (fileUpload.HasFile)
            {
                string fileContentType = fileUpload.PostedFile.ContentType;
                if ((fileContentType == "application/msword" || fileContentType == "application/pdf"
                || fileContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
                {
                    int id = wrApp.AddProposalTracker(entity);
                    if (id > 0)
                    {
                        FilesEntity model = FileFactory.CreateFileEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                        model.FilePath = entity.WorkScope;

                        model.ProposalTrackerId = id;
                        model.ContentType = fileUpload.PostedFile.ContentType;
                        model.FileID = 0;
                        model.FileSize = fileUpload.PostedFile.ContentLength;
                        model.FileTitle = Path.GetFileName(fileUpload.FileName);
                        model.IsDelete = false;
                        model.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
                        model.SourceType = (int)FileSourceType.WorkRequestScope;
                        model.ThumbPath = Path.GetFileName(fileUpload.PostedFile.FileName);
                        result = fileApp.AddFile(model) > 0 ? true : false;
                        Redirect("Index.aspx");
                    }
                    else
                    {
                        ShowFailMessageToClient(wrApp.BrokenRuleMessages);
                    }
                }
                else
                {
                    ShowFailMessageToClient("Please select a file to upload ( *.doc, *.docx, *.pdf)");
                }
            }
            else
            {
                ShowFailMessageToClient("Please upload Work Scope");
            }

            */
        }


        private ProposalTrackerEntity GetEntity()
        {
            ProposalTrackerEntity model = new ProposalTrackerEntity();

            // basic infomation
            model.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
            model.Status = Convert.ToInt32(ddlStatus.SelectedValue);
            model.Title = txtTitle.Text.Trim();
            model.Description = txtDescription.Text.Trim();

            model.ProposalSentTo = txtProposalSentTo.Text.Trim();
            DateTime proposalSentOn;
            if (DateTime.TryParse(txtProposalSentOn.Text.Trim(), out proposalSentOn))
            {
                model.ProposalSentOn = proposalSentOn;
            }
            else
            {
                model.ProposalSentOn = null;
            }
            model.PONo = txtPO.Text.Trim();
            model.ApprovedBy = txtApprovedBy.Text.Trim();
            DateTime approvedOn;
            if (DateTime.TryParse(txtApprovedOn.Text.Trim(), out approvedOn))
            {
                model.ApprovedOn = approvedOn;
            }
            else
            {
                model.ApprovedOn = null;
            }
            model.PoTotalLessThenProposalTotal = chkLessThen.Checked;
            model.InvoiceNo = "";
            model.InvoiceSentOn = null;
            //DateTime dueDate;
            //if (DateTime.TryParse(txtDueDate.Text.Trim(), out dueDate))
            //{
            //    model.DueDate = dueDate;
            //}
            //else
            //{
            //    model.DueDate = null;
            //}



            if (fileUpload.HasFile)
            {
                string fileContentType = fileUpload.PostedFile.ContentType;
                model.WorkScope = UtilFactory.Helpers.FileHelper.SaveUploadFiles("WorkScope", model.ProjectID, fileUpload.PostedFile);
                model.WorkScopeDisplayName = fileUpload.FileName;
            }

            model.RequestNo = "";
            //model.RequestNo = wrApp.GetProposalTrackerNo();
            model.CreatedOn = DateTime.Now;
            model.CreatedBy = UserInfo.UserID;
            return model;
        }
    }
}