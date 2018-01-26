using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.FileModel;
using System.Text.RegularExpressions;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Documents
{
    public partial class CompanyFiles : BaseWebsitePage
    {
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

                ltlWhosCompany.Text = "";
            }
            else
            {
                List<CompanysEntity> list = comApp.GetAllCompanies();
                list = list.FindAll(c => c.ID > 1);
                list.BindDropdown<CompanysEntity>(ddlCompany, "CompanyName", "ID", "All", "0");
                ltlWhosCompany.Text = " - All company files";
            }
        }
        private void InitControl()
        {
            SearchFilesRequest request = new SearchFilesRequest(SearchFileType.Company, true, "FileTitle", "ASC");
            request.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
            request.CurrentPage = anpFiles.CurrentPageIndex;
            request.PageCount = anpFiles.PageSize;
            request.Keyword = txtKeyword.Text.NoHTML();
            request.CompanyID = int.Parse(ddlCompany.SelectedValue);
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
