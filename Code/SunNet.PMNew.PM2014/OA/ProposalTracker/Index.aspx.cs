using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Utils;
using System.Text;
using SunNet.PMNew.Entity.CompanyModel;

namespace SunNet.PMNew.PM2014.OA.ProposalTracker
{
    public partial class Index : BasePage
    {
        protected override string DefaultOrderBy
        {
            get
            {
                return "ProposalTrackerID";
            }
        }

        protected override string DefaultDirection
        {
            get { return "DESC"; }
        }

        ProposalTrackerApplication wrApp;

        protected void Page_Load(object sender, EventArgs e)
        {
            wrApp = new ProposalTrackerApplication();
            if (!IsPostBack)
            {
                InitProject();
                ddlProject.SelectedValue = QS("project", 0).ToString();
                ddlStatus.SelectedValue = QS("status", 0).ToString();
                ddlCompany.SelectedValue = QS("companyId", 0).ToString();
                txtStartDate.Text = QS("start").Trim();
                txtEndDate.Text = QS("end").Trim();

                //ddlPayment.SelectedValue = QS("payment", 0).ToString();
                InitControl();
            }
        }

        private void InitControl()
        {
            int userId = 0;
            if (UserInfo.Role == RolesEnum.CLIENT)
                userId = UserInfo.UserID;
            DateTime? beginTime = null;
            DateTime? endTime = null;

            if (!string.IsNullOrEmpty(QS("start").Trim()))
            {
                DateTime start = DateTime.MinValue;
                DateTime.TryParse(QS("start").Trim(), out start);
                if (start > DateTime.MinValue)
                {
                    start = DateTime.Parse(start.ToString("yyyy-MM-dd 00:00:00"));
                    beginTime = start;
                }
            }
            if (!string.IsNullOrEmpty(QS("end").Trim()))
            {
                DateTime end = DateTime.MaxValue;
                DateTime.TryParse(QS("end").Trim(), out end);
                if (end < DateTime.MaxValue)
                {
                    end = DateTime.Parse(end.ToString("yyyy-MM-dd 23:59:59"));
                    endTime = end;
                }
            }

            SearchProposalTrackerRequest request = wrApp.GetSearchProposalTrackers(QS("keywork").Trim(), QS("project", 0),
                                     QS("status", 0), QS("companyId", 0), QS("payment", 0), userId, beginTime, endTime
                                    , OrderBy, OrderDirection, anpWaitting.PageSize, CurrentPageIndex);
            rptWR.DataSource = request.ResultList;
            rptWR.DataBind();
            anpWaitting.RecordCount = request.ResultCount;
        }

        private void InitProject()
        {
            ProjectApplication projApp = new ProjectApplication();
            List<ProjectDetailDTO> list = projApp.GetUserProjects(UserInfo);
            list.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "All", "0");

            CompanyApplication comApp = new CompanyApplication();
            List<CompanysEntity> listCom = comApp.GetAllCompanies();
            listCom.BindDropdown(ddlCompany, "CompanyName", "ComID", "All", "0");

        }

        public string ShowPriorityImgByDevDate(object date)
        {
            string imgString = "";
            if (date == null)
            {
                return imgString;
            }
            DateTime Now = DateTime.Now.Date;

            TimeSpan Diff = Convert.ToDateTime(date).Subtract(Now);

            int DiffDate = Diff.Days;

            if (DiffDate <= 3 && DiffDate > 0)
            {
                imgString = "<img src='/Images/icons/02.gif' title='Due Day is 3 days before today'  />";
            }
            else if (DiffDate == 0)
            {
                imgString = "<img src='/Images/icons/03.gif' title ='Due Date is today' />";
            }
            else if (Convert.ToDateTime(date) > UtilFactory.Helpers.CommonHelper.GetDefaultMinDate() && DiffDate < 0)
            {
                imgString = "<img src='/Images/icons/01.gif' title='Passed Due Date' />";
            }
            return imgString;
        }


        protected void rptWR_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal labelID = e.Item.FindControl("lblWID") as Literal;
                Literal labelHour = e.Item.FindControl("lblHours") as Literal;
                decimal needHours = wrApp.GetProposalTrackerHours(Convert.ToInt32(labelID.Text));
                string link = string.Empty;
                if (!decimal.Equals(0, needHours))
                {
                    link = @"<a href='###' onclick='navigateToTimesheetReport(event," + labelID.Text + ")'>"
                       + needHours + "</a>";
                }
                labelHour.Text = link;

            }
        }

        protected string BuilderDown(string file, int id, string name)
        {
            if (string.IsNullOrEmpty(file))
                return "";
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("<a href=\"/do/DoDownWrokRequestFile.ashx?ID={0}\" title='{1}' target=\"_blank\">"
                     , id, name);
                sb.AppendFormat("<img src=\"/Images/icons/download.png\" alt='{0}' /></a>"
                    , name);
                return sb.ToString();
            }
        }
    }
}
