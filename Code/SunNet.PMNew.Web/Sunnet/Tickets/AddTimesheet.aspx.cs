using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TimeSheetModel;
namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class AddTimesheet : BaseWebsitePage
    {
        ProjectApplication projApp;
        TimeSheetApplication tsApp;
        TicketsApplication tickApp;
        CateGoryApplication cgApp;
        protected string GetPercentage(object ticketid)
        {
            return "100";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            cgApp = new CateGoryApplication();
            tickApp = new TicketsApplication();
            tsApp = new TimeSheetApplication();
            projApp = new ProjectApplication();

            try
            {
                if (!string.IsNullOrEmpty(Request.Params["date"]))
                {
                    this.SelectedDate = Convert.ToDateTime(Request.QueryString["date"]);
                    if (!TimeSheetTicket.CanEdit(this.SelectedDate))
                    {
                        Alert("This date is invalid", "ListTimesheet.aspx");
                    }
                }
                else
                {
                    this.SelectedDate = DateTime.Now;
                }
            }
            catch
            {
                this.SelectedDate = DateTime.Now;
            }
            if (!IsPostBack)
            {
                InitControls();
                this.Title = string.Format("Write Timesheet - {0}", this.SelectedDate.ToString("MM/dd/yyyy"));
            }
        }
        protected DateTime SelectedDate
        {
            get;
            set;

        }
        private void InitControls()
        {
            CateGoryApplication cgApp = new CateGoryApplication();
            List<CateGoryEntity> list = cgApp.GetCateGroyListByUserID(UserInfo.ID);
            list.BindDropdown(ddlCategorys, "Title", "ID", "Please Select", "0");
            if (list != null && list.Count == 1)
            {
                ddlCategorys.AddEmptyItem(0, "Please Select", "0", true);
            }
            List<ProjectDetailDTO> listProj = projApp.GetUserProjects(UserInfo);
            if (listProj != null && listProj.Count > 0)
            {
                listProj.BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "Please Select", "0");
            }
            
            if (listProj != null && listProj.Count == 1)
            {
                ddlProject.AddEmptyItem(0, "Please Select", "0", true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tsApp.SubmitTimeSheets(this.SelectedDate, UserInfo.ID))
            {
                ddlCategorys.Items.Clear();
                ddlCategorys.AddEmptyItem(0, "Submitted", "0", true);
                ddlProject.Items.Clear();
                ddlProject.AddEmptyItem(0, "Submitted", "0", true);
                this.ShowSuccessMessageToClient(false, false);

            }
            else
            {
                this.ShowFailMessageToClient(tsApp.BrokenRuleMessages, false);
            }
        }

    }
}
