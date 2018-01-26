using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.WorkRequestModel;
using SunNet.PMNew.Entity.FileModel;
using StructureMap;
using System.IO;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class AddWorkRequest : BaseAscx
    {

        UserApplication userApp = new UserApplication();
        WorkRequestApplication wqApp = new WorkRequestApplication();
        FileApplication fileApp = new FileApplication();
        public UsersEntity UserInfo
        {
            get
            {
                IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                string userID = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("LoginUserID")));
                if (string.IsNullOrEmpty(userID))
                {
                    return null;
                }
                UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LoginUserID"), 30);

                int id = int.Parse(userID);
                UsersEntity model = userApp.GetUser(id);
                return model;
            }
        }

        public WorkRequestEntity wrEntity { get; set; }

        public bool IsAdd
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControls();
                if (wrEntity != null)
                {
                    BindDataModel(wrEntity);
                }
            }
        }

        public string WorkRequestID
        {
            get
            {
                if (ViewState["WorkRequestID"] != null)
                    return ViewState["WorkRequestID"].ToString();
                return "";
            }
            set
            {
                ViewState["WorkRequestID"] = value;
            }
        }
        private void BindDataModel(WorkRequestEntity info)
        {
            this.ddlProject.SelectedValue = info.ProjectID.ToString();
            this.ddlProject.Enabled = false;
            this.ddlProject.Style.Add("background-color", "#EEEFED");
            this.ddlPayment.SelectedValue = info.Payment.ToString();
            this.txtRequestNo.Text = info.RequestNo;
            this.txtInvoiceNo.Text = info.InvoiceNo;
            this.ddlStatus.SelectedValue = info.Status.ToString();
            if (info.DueDate == null)
            {
                this.txtDueDate.Text = "";
            }
            else
            {
                this.txtDueDate.Text = info.DueDate.Value.ToString("MM/dd/yyyy");
            }
            this.txtTitle.Text = info.Title;
            this.txtDescription.Text = info.Description;
            lblFile.Visible = String.IsNullOrEmpty(info.WorkScopeDisplayName) ? false : true;
            lblFile.Text = string.Format(@"<a href='Download.aspx?FileName={0}&FilePath={1}' 
                    target='_blank'>{0}</a>", info.WorkScopeDisplayName, info.WorkScope);
        }

        private void InitControls()
        {
            InitProject();
            //txtRequestNo.Enabled = false;
        }

        private void InitProject()
        {
            ProjectApplication projApp = new ProjectApplication();

            List<ProjectDetailDTO> list = projApp.GetUserProjects(UserInfo);
            if (!string.IsNullOrEmpty(WorkRequestID))
            {
                WorkRequestEntity workRequestEntity = wqApp.Get(int.Parse(WorkRequestID));
                ProjectsEntity projectsEntity = projApp.Get(workRequestEntity.ProjectID);
                if (!projApp.IsProjectHasPM(projectsEntity.ProjectID, UserInfo.UserID))
                {
                    list.Add(new ProjectDetailDTO() { Title = projectsEntity.Title, ID = projectsEntity.ProjectID });
                }
            }
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
            bool result = true;
            WorkRequestEntity entity = GetEntity();
            if (IsAdd)
            {
                if (fileProject.HasFile)
                {
                    string fileContentType = fileProject.PostedFile.ContentType;
                    if ((fileContentType == "application/msword" || fileContentType == "application/pdf"
                    || fileContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
                    {
                        int id = wqApp.AddWorkRequest(entity);
                        if (id > 0)
                        {
                            FilesEntity model = FileFactory.CreateFileEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                            model.FilePath = UtilFactory.Helpers.FileHelper.SaveUploadFiles("WorkRequest", id, fileProject.PostedFile); ;

                            model.WorkRequestId = id;
                            model.ContentType = fileProject.PostedFile.ContentType;
                            model.FileID = 0;
                            model.FileSize = fileProject.PostedFile.ContentLength;
                            model.FileTitle = Path.GetFileName(fileProject.FileName);
                            model.IsDelete = false;
                            model.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
                            model.SourceType = (int)FileSourceType.WorkRequestScope;
                            model.ThumbPath = Path.GetFileName(fileProject.PostedFile.FileName);
                            result = fileApp.AddFile(model) > 0 ? true : false;
                            BaseWebsitePage.ShowSuccessMessageToClient();
                        }
                        else
                        {
                            BaseWebsitePage.ShowFailMessageToClient(wqApp.BrokenRuleMessages);
                        }
                    }
                    else
                    {
                        BaseWebsitePage.ShowFailMessageToClient("Please select a file to upload ( *.doc, *.docx, *.pdf)");
                    }
                }
                else
                {
                    BaseWebsitePage.ShowFailMessageToClient("Please upload Work Scope");
                }

            }
            //edit
            else
            {
                if (fileProject.HasFile)
                {
                    string fileContentType = fileProject.PostedFile.ContentType;
                    if ((fileContentType == "application/msword" || fileContentType == "application/pdf"
                    || fileContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
                    {
                        if (wqApp.UpdateWorkRequest(entity))
                        {
                            FilesEntity model = FileFactory.CreateFileEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                            model.FilePath = UtilFactory.Helpers.FileHelper.SaveUploadFiles("WorkRequest", entity.WorkRequestID, fileProject.PostedFile); ;

                            model.WorkRequestId = entity.WorkRequestID;
                            model.ContentType = fileProject.PostedFile.ContentType;
                            model.FileID = 0;
                            model.FileSize = fileProject.PostedFile.ContentLength;
                            model.FileTitle = Path.GetFileName(fileProject.FileName);
                            model.IsDelete = false;
                            model.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
                            model.SourceType = (int)FileSourceType.WorkRequestScope;
                            model.ThumbPath = Path.GetFileName(fileProject.PostedFile.FileName);
                            result = fileApp.AddFile(model) > 0 ? true : false;
                            BaseWebsitePage.ShowSuccessMessageToClient(true, false);
                        }
                    }
                    else
                    {
                        BaseWebsitePage.ShowFailMessageToClient("Please select a file to upload ( *.doc, *.docx, *.pdf)");
                    }
                }
                else
                {
                    if (!lblFile.Visible)
                    {
                        BaseWebsitePage.ShowFailMessageToClient("Please upload Work Scope");

                    }
                    else
                    {
                        if (wqApp.UpdateWorkRequest(entity))
                        {
                            BaseWebsitePage.ShowSuccessMessageToClient(true, false);
                        }
                        else
                        {
                            BaseWebsitePage.ShowFailMessageToClient(wqApp.BrokenRuleMessages);
                        }
                    }
                }

            }
        }


        private WorkRequestEntity GetEntity()
        {
            WorkRequestEntity model = new WorkRequestEntity();

            int wid = QS("id", 0);
            if (wid > 0)
            {
                model = wqApp.Get(wid);
            }
            // basic infomation
            model.ProjectID = Convert.ToInt32(ddlProject.SelectedValue);
            model.Payment = Convert.ToInt32(this.ddlPayment.SelectedValue);
            model.InvoiceNo = txtInvoiceNo.Text.Trim();
            model.Status = Convert.ToInt32(ddlStatus.SelectedValue);
            DateTime dueDate;
            if (DateTime.TryParse(txtDueDate.Text.Trim(), out dueDate))
            {
                model.DueDate = dueDate;
            }
            else
            {
                model.DueDate = null;
            }
            model.Title = txtTitle.Text.Trim();
            model.Description = txtDescription.Text.Trim();

            if (fileProject.HasFile)
            {
                string fileContentType = fileProject.PostedFile.ContentType;
                model.WorkScope = UtilFactory.Helpers.FileHelper.SaveUploadFiles("WorkScope", 0, fileProject.PostedFile);
                model.WorkScopeDisplayName = fileProject.FileName;
            }
            if (IsAdd)
            {
                model.RequestNo = wqApp.GetWorkRequestNo();
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = UserInfo.UserID;
            }
            else
            {
                model.WorkRequestID = Convert.ToInt32(WorkRequestID);
                model.ModifyOn = DateTime.Now;
                model.ModifyBy = UserInfo.UserID;
            }
            return model;
        }


    }
}