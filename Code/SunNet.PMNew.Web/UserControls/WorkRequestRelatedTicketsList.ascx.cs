using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class WorkRequestRelatedTicketsList : BaseAscx
    {
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

        public string ProjectID
        {
            get
            {
                if (ViewState["ProjectID"] != null)
                    return ViewState["ProjectID"].ToString();
                return "";
            }
            set
            {
                ViewState["ProjectID"] = value;
            }
        }


        public string ProjectName
        {
            get
            {
                if (ViewState["ProjectName"] != null)
                    return ViewState["ProjectName"].ToString();
                return "";
            }
            set
            {
                ViewState["ProjectName"] = value;
            }
        }
        int page = 1;
        int recordCount;
        WorkRequestApplication app = new WorkRequestApplication();
        ProjectApplication proApp = new ProjectApplication();
        UserApplication userApp = new UserApplication();
        List<TicketsEntity> list = new List<TicketsEntity>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindList();

                this.WorkRequestID = QS("id", 0).ToString();
                decimal hour = app.GetWorkRequestHours(Convert.ToInt32(this.WorkRequestID));

                string link = @"<a href='/sunnet/Reports/TimeSheet.aspx?WID=" + this.WorkRequestID + "'>Total Hours:"
                    + hour + "</a>";
                lblHour.Text = link;
                lblProjectName.Text = proApp.Get(Convert.ToInt32(this.ProjectID)).Title;
            }
        }

        private void BindList()
        {
            list = app.GetAllRelation(Convert.ToInt32(this.WorkRequestID));
            if (null != list && list.Count > 0)
            {
                this.rptTicketsList.DataSource = list;
                this.rptTicketsList.DataBind();
            }
            else
            {
                //this.trNoTickets.Visible = true;
            }
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


        public string ShowPriorityImgByDevDate(string date)
        {
            string imgString = "";

            DateTime Now = DateTime.Now.Date;

            TimeSpan Diff = Convert.ToDateTime(date).Subtract(Now);

            int DiffDate = Diff.Days;

            if (DiffDate <= 3 && DiffDate > 0)
            {
                imgString = "<img src='/icons/02.gif' />";
            }
            else if (DiffDate == 0)
            {
                imgString = "<img src='/icons/03.gif' />";
            }
            else if (Convert.ToDateTime(date) > UtilFactory.Helpers.CommonHelper.GetDefaultMinDate() && DiffDate < 0)
            {
                imgString = "<img src='/icons/01.gif' />";
            }

            return imgString;
        }

    }
}