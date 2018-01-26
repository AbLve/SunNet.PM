using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using StructureMap;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;
using System.Drawing;
using System.IO;
using SunNet.PMNew.Entity.FileModel;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class CompanyEdit : BaseAscx
    {
        CompanyApplication companyApp;
        private void SetControlsStatus()
        {
            bool isReadOnly = BaseWebsitePage.ISReadOnlyRole;

            savebasicinfo.Visible = !isReadOnly;
            fileform.Visible = !isReadOnly;
            logoform.Visible = !isReadOnly;
            ddlState.Enabled = !isReadOnly;

            txtAddress1.ReadOnly = isReadOnly;
            txtAddress2.ReadOnly = isReadOnly;
            txtCity.ReadOnly = isReadOnly;
            txtCompanyName.ReadOnly = isReadOnly;
            txtFax.ReadOnly = isReadOnly;
            txtFileTitle.ReadOnly = isReadOnly;
            txtPhone.ReadOnly = isReadOnly;

            txtWebsite.ReadOnly = isReadOnly;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            companyApp = new CompanyApplication();
            if (!IsPostBack)
            {
                InitControls();
                SetControlsStatus();
            }
        }
        public int CompanyID { get; set; }
        private void InitControls()
        {
            int id = CompanyID;
            CompanysEntity model = companyApp.GetCompany(id);

            txtAddress1.Text = model.Address1;
            txtAddress2.Text = model.Address2;
            //model.AssignedSystemUrl = "http://client.sunnet.us";
            txtCity.Text = model.City;
            txtCompanyName.Text = model.CompanyName;
            txtFax.Text = model.Fax;
            txtPhone.Text = model.Phone;
            ListItem state = ddlState.Items.FindByText(model.State);
            if (state != null)
            {
                state.Selected = true;
            }
            txtWebsite.Text = model.Website;

            InitControl(model);

            InitProjects();
            InitUsers();

            if (CompanyID == 1)
            {
                phFiles.Visible = false;
            }
            else
            {
                phFiles.Visible = true;
                InitFiles();
            }
        }
        private void InitControl(CompanysEntity model)
        {
            string noImg = "    - <font color='red'> Please upload a logo image in format JPG PNG GIF or BMP  , size(width:126px,height:39px)</font>";
            try
            {
                string fileName = Server.MapPath(model.Logo);
                if (File.Exists(fileName))
                {
                    System.Drawing.Image image = Bitmap.FromFile(Server.MapPath(model.Logo));
                    if (image.Width > 126)
                    {
                        imgLogo.Width = 126;
                        imgLogo.Height = image.Height * 126 / image.Width;
                    }
                    imgLogo.ImageUrl = model.Logo.Replace("//", "/");
                    ltlNoLogo.Text = "";
                    if (model.Logo.IndexOf("logomain.jpg") >= 0)
                    {
                        SetNoLogo();
                        iBtnDeleteLogo.Visible = false;
                        ltlNoLogo.Text = noImg;
                    }
                    else
                    {
                        iBtnDeleteLogo.Visible = true;
                    }
                }
                else
                {
                    SetNoLogo();
                    ltlNoLogo.Text = noImg;
                }
            }
            catch (OutOfMemoryException ooex)
            {
                SetNoLogo();
                iBtnDeleteLogo.Visible = false;
                ltlNoLogo.Text = noImg;
            }
        }
        private void SetNoLogo()
        {
            iBtnDeleteLogo.Visible = false;
            imgLogo.Visible = true;
            imgLogo.ImageUrl = "/images/logomain.jpg";
            imgLogo.ToolTip = "Your logo may be lost,please upload again.";
        }
        private void InitUsers()
        {
            int id = CompanyID;
            SearchUsersRequest request = new SearchUsersRequest(
                SearchUsersType.Company, false, " FirstName ", " ASC ");
            request.CompanyID = id;

            SearchUserResponse response = companyApp.SearchUsers(request);
            if (response.ResultList.Count <= 0)
            {
                trNoUser.Visible = true;
            }
            else
            {
                rptUsers.DataSource = response.ResultList;
                rptUsers.DataBind();
                trNoUser.Visible = false;
            }

        }
        private void InitFiles()
        {
            FileApplication fileApp = new FileApplication();
            SearchFilesRequest request = new SearchFilesRequest(SearchFileType.Company, false, "FileTitle", "ASC");
            request.CompanyID = CompanyID;
            request.IsPublic = BaseWebsitePage.UserInfo.Role == RolesEnum.CLIENT;
            List<FileDetailDto> list = fileApp.GetFiles(request);

            if (list == null || list.Count == 0)
            {
                trNoFiles.Visible = true;
            }
            else
            {
                trNoFiles.Visible = false;
                rptFiles.DataSource = list;
                rptFiles.DataBind();
            }
        }
        private void InitProjects()
        {
            int id = CompanyID;

            ProjectApplication projApp = new ProjectApplication();
            SearchProjectsRequest request = new SearchProjectsRequest(SearchProjectsType.Company, false, " Title ", " ASC ");
            request.CompanyID = id;

            SearchProjectsResponse response = projApp.SearchProjects(request);
            if (response.ResultList == null || response.ResultList.Count <= 0)
            {
                trNoProject.Visible = true;
            }
            else
            {
                rptProjects.DataSource = response.ResultList;
                rptProjects.DataBind();
                trNoProject.Visible = false;
            }
        }
        protected CompanysEntity GetEntity()
        {
            int id = CompanyID;
            CompanysEntity model = companyApp.GetCompany(id);
            model.Address1 = txtAddress1.Text;
            model.Address2 = txtAddress2.Text;
            //model.AssignedSystemUrl = "http://client.sunnet.us";
            model.City = txtCity.Text;
            model.CompanyName = txtCompanyName.Text;
            model.CreateUserName = UserInfo.UserName;
            model.Fax = txtFax.Text;
            //model.Logo = "/Images/nologo.jpg";
            model.Phone = txtPhone.Text;
            model.State = ddlState.SelectedItem.Text;
            //model.Status = "ACTIVE";
            model.Website = txtWebsite.Text;

            return model;
        }
        protected void btnSaveFiles_Click(object sender, EventArgs e)
        {
            if (fileProject.HasFile)
            {
                FilesEntity model = FileFactory.CreateFileEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                model.FilePath = UtilFactory.Helpers.FileHelper.SaveUploadFiles("Company", CompanyID, fileProject.PostedFile); ;

                model.CompanyID = CompanyID;
                model.ContentType = Path.GetExtension(fileProject.FileName);
                model.FileID = 0;
                model.FileSize = fileProject.PostedFile.ContentLength;
                if (string.IsNullOrEmpty(txtFileTitle.Text))
                {
                    model.FileTitle = Path.GetFileName(fileProject.FileName);
                    model.FileTitle = model.FileTitle.Substring(0, model.FileTitle.LastIndexOf("."));
                }
                else
                {
                    model.FileTitle = txtFileTitle.Text;
                }
                model.IsDelete = false;
                model.IsPublic = BaseWebsitePage.UserInfo.Role == RolesEnum.CLIENT;
                model.SourceType = (int)FileSourceType.Company;
                model.ThumbPath = Path.GetFileName(fileProject.PostedFile.FileName);
                model.IsDelete = false;

                FileApplication fileApp = new FileApplication();
                int result = fileApp.AddFile(model);
                if (result <= 0)
                {
                    BaseWebsitePage.ShowFailMessageToClient(fileApp.BrokenRuleMessages, false);
                }
                else
                {
                    BaseWebsitePage.ShowSuccessMessageToClient(false, false);
                    InitFiles();
                    txtFileTitle.Text = "";
                }
            }
        }

        private bool CheckInput(out string msg)
        {
            msg = string.Empty;
            if (ddlState.SelectedValue == "0")
            {
                msg = "Please select a state";
                return false;
            }
            return true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!CheckInput(out msg))
            {
                BaseWebsitePage.ShowMessageToClient(msg, 2, false, false);
                return;
            }
            CompanysEntity model = GetEntity();

            if (companyApp.UpdateCompany(model))
            {
                InitControl(model);
                BaseWebsitePage.ShowSuccessMessageToClient(false, false);
            }
            else
            {
                BaseWebsitePage.ShowFailMessageToClient(companyApp.BrokenRuleMessages, false);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            int id = CompanyID;
            CompanysEntity model = companyApp.GetCompany(id);
            if (fileLogo.HasFile)
            {
                if (".jpg.png.gif.bmp".IndexOf(Path.GetExtension(fileLogo.FileName).ToLower()) >= 0)
                {
                    model.Logo = UtilFactory.Helpers.FileHelper.SaveUploadFiles("Company", model.ID, fileLogo.PostedFile);
                    if (companyApp.UpdateCompany(model))
                    {
                        InitControl(model);
                        BaseWebsitePage.ShowSuccessMessageToClient(false, false);
                    }
                    else
                    {
                        BaseWebsitePage.ShowFailMessageToClient();
                    }
                }
                else
                {
                    BaseWebsitePage.ShowMessageToClient("The image format is incorrect!", 0, false, false);
                }
            }
        }

        protected void iBtnDeleteLogo_Click(object sender, ImageClickEventArgs e)
        {
            int id = CompanyID;
            CompanysEntity model = companyApp.GetCompany(id);
            model.Logo = "/Images/nologo.jpg";
            InitControl(model);
            if (companyApp.UpdateCompany(model))
            {
                BaseWebsitePage.ShowSuccessMessageToClient(false, false);
            }
            else
            {
                BaseWebsitePage.ShowFailMessageToClient();
            }
        }
    }
}