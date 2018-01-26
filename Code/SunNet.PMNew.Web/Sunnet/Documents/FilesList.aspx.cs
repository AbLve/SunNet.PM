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
    public partial class FilesList : BaseWebsitePage
    {
        protected string TicketUrl
        {
            get
            {
                if (UserInfo.Role == RolesEnum.CLIENT)
                {
                    return "Clients/TicketsDetail.aspx";
                }
                return "Tickets/TicketDetail.aspx";
            }
        }
        protected string GetHtml(object type, object text, object id)
        {
            FileSourceType sourceType = (FileSourceType)type;
            switch (sourceType)
            {
                case FileSourceType.Project:
                    if (UserInfo.Role == RolesEnum.CLIENT)
                    {
                        return text.ToString();
                    }
                    if (!CheckRoleCanAccessPage("/Sunnet/Projects/ViewProject.aspx"))
                    {
                        return text.ToString();
                    }
                    return string.Format(" <a href='###' onclick='OpenEditObject({0})' >{1}</a>", id.ToString(), text.ToString());
                case FileSourceType.Ticket:
                    return string.Format(" <a href='###' onclick='OpenTicketDetail({0})' >{1}</a>", id.ToString(), text.ToString());
                case FileSourceType.FeedBack:
                    return string.Format(" <a href='###' onclick='OpenTicketDetailFeedBack({0})' >{1}</a>", id.ToString(), text.ToString());
                default: break;
            }
            return string.Empty;
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

            SearchFilesRequest request = new SearchFilesRequest(SearchFileType.TicketAndFeedback, true, hidOrderBy.Value, hidOrderDirection.Value);
            request.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
            request.CurrentPage = anpFiles.CurrentPageIndex;
            request.PageCount = anpFiles.PageSize;
            Regex regticketcode = new Regex("^[BbRr]");
            request.Keyword = regticketcode.Replace(txtKeyword.Text, "").NoHTML();
            request.CompanyID = int.Parse(ddlCompany.SelectedValue);
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.UserID = UserInfo.ID;
            request.KeywordType = SearchKeywordType.All;
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
                rptTickets.Visible = true;
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
