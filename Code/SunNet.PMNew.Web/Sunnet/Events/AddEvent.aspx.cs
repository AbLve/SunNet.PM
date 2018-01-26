using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    //显然添加Event是与Project和user相关的.
    public partial class AddEvent : BaseWebsitePage
    {
        private ProjectApplication projApp;

        protected void Page_Load(object sender, EventArgs e)
        {
            projApp = new ProjectApplication();
            if (!Page.IsPostBack)
            {
                DateTime eventDate = QS("Date", DateTime.Now);
                txtFrom.Text = eventDate.ToString("MM/dd/yyyy");
                txtTo.Text = eventDate.ToString("MM/dd/yyyy");
                txtEndDate.Text = eventDate.ToString("MM/dd/yyyy");
                projApp.GetUserProjects(UserInfo).BindDropdown<ProjectDetailDTO>(ddlProjects, "Title", "ProjectID", "Please select...", "-1");
                int searchedProjectID = QS("pid", -1);
                ddlProjects.SelectedValue = searchedProjectID.ToString();
            }
        }

        public void SaveEvent(EventsView model)
        {
            //string eventIds;
            //if (new EventsApplication().AddEvents(model, out eventIds))
            //{
            //    ShowSuccessMessageToClient("The event has been added.", true, true, "350", "50");
            //}
            //else
            //{
            //    ShowFailMessageToClient("Add event Fail", "350", "50");
            //}
        }

        private EventsView ConstructEventsView()
        {
            EventsView eventsView = new EventsView();
            eventsView.Alert = (AlertType)int.Parse(ddlAlert.SelectedItem.Value);
            eventsView.AllDay = chkAllDay.Checked;
            eventsView.CreatedBy = UserInfo.UserID;
            eventsView.CreatedOn = DateTime.Now;
            eventsView.Details = txtDetails.Text.NoHTML();
            eventsView.End = (EndType)int.Parse(ddlEnd.SelectedValue);
            eventsView.EndDate = DateTime.Parse(txtEndDate.Text);
            //model.FamilyID = FamilyID;
            eventsView.FromDay = DateTime.Parse(txtFrom.Text);
            eventsView.FromTime = txtFromTime.Text;
            eventsView.FromTimeType = int.Parse(selectFromType.SelectedValue);
            eventsView.Name = txtName.Text.NoHTML();
            eventsView.ProjectID = int.Parse(ddlProjects.SelectedValue);
            eventsView.ToDay = DateTime.Parse(txtTo.Text);
            eventsView.ToTime = txtToTime.Text;
            eventsView.ToTimeType = int.Parse(selectToType.SelectedValue);
            eventsView.Repeat = (RepeatType)int.Parse(selectRepeat.SelectedValue);
            eventsView.RoleIDs = ((int)Privates.OnlyMe).ToString();
            eventsView.Times = int.Parse(txtTimes.Text);
            eventsView.UserIds = "";
            eventsView.Where = txtWhere.Text.NoHTML();

            eventsView.Icon = string.IsNullOrEmpty(Icon.Value) ? 0 : int.Parse(Icon.Value);
            return eventsView;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveEvent(ConstructEventsView());
        }
    }
}
