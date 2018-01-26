using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using System.Text.RegularExpressions;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProjectModel;

namespace SunNet.PMNew.Web.Sunnet.Documents
{
    public partial class ProjectFiles : BaseWebsitePage
    {
        protected string GetHtml(object projecttitle, object id)
        {
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                return projecttitle.ToString();
            }
            if (!CheckRoleCanAccessPage("/Sunnet/Projects/ViewProject.aspx"))
            {
                return projecttitle.ToString();
            }
            return string.Format(" <a href='###' onclick='OpenEditObject({0})' >{1}</a>", id.ToString(), projecttitle.ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitCompany();
                InitControl();
            }
        }
        private void InitCompany()
        {
            CompanyApplication comApp = new CompanyApplication();
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                CompanysEntity model = comApp.GetCompany(UserInfo.CompanyID);
                ddlCompany.Items.Add(new ListItem(model.CompanyName, model.ID.ToString()));
                tdCompanyLabel.Visible = false;
                tdCompanyDdl.Visible = false;
            }
            else
            {
                List<CompanysEntity> list = comApp.GetAllCompanies();
                list.BindDropdown<CompanysEntity>(ddlCompany, "CompanyName", "ID", "All", "0");
            }
            ddlCompany_OnSelectedIndexChanged(null, null);
        }
        private void InitControl()
        {
            SearchFilesRequest request = new SearchFilesRequest(SearchFileType.Project, true, "FileTitle", "ASC");
            request.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
            request.CurrentPage = anpFiles.CurrentPageIndex;
            request.PageCount = anpFiles.PageSize;
            request.Keyword = txtKeyword.Text.NoHTML();
            request.CompanyID = int.Parse(ddlCompany.SelectedValue);
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            FileApplication fileApp = new FileApplication();
            List<FileDetailDto> list = fileApp.GetFiles(request);
            if (list == null || list.Count == 0)
            {
                rptTickets.Visible = false;
                trNoTickets.Visible = true;
            }
            else
            {
                rptTickets.DataSource = list;
                rptTickets.DataBind();
                trNoTickets.Visible = false;
                anpFiles.RecordCount = request.RecordCount;
            }
        }
        protected void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCompany.SelectedValue == "0")
            {
                ddlProject.Items.Clear();
                ddlProject.Items.Add(new ListItem("All", "0"));
            }
            else
            {
                int comID = int.Parse(ddlCompany.SelectedValue);
                ProjectApplication projApp = new ProjectApplication();
                List<ProjectDetailDTO> listProj = projApp.GetUserProjects(UserInfo);
                List<ProjectDetailDTO> listcomProj = listProj.FindAll(p => p.CompanyID == comID);
                listcomProj.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "All", "0");
            }
        }
        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            anpFiles.CurrentPageIndex = 1;
            InitControl();
        }
        protected void anpFiles_PageChanged(object sender, EventArgs e)
        {
            InitControl();
        }
    }
}
