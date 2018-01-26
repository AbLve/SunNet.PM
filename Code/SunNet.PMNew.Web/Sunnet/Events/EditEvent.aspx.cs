using SunNet.PMNew.App;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Web.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class EditEvent : BaseWebsitePage
    {
        private ProjectApplication projApp;

        protected void Page_Load(object sender, EventArgs e)
        {
            projApp = new ProjectApplication();

            if (!Page.IsPostBack)
            {
                int id = QS("ID", 0);
                int canEdit = QS("c", 0);
                if (canEdit == 2)
                {
                    trBtns.Visible = false;
                    ltrlControlTitle.Text = "View Event";
                }
                EventEntity eventEntity = new EventsApplication().GetEventInfo(id);
                txtName.Text = eventEntity.Name;
                txtDetails.Text = eventEntity.Details;
                txtWhere.Text = eventEntity.Where;
                chkAllDay.Checked = eventEntity.AllDay;
                if (chkAllDay.Checked)
                {
                    txtFromTime.Style.Add("display", "none");
                    selectFromType.Style.Add("display", "none");
                    txtToTime.Style.Add("display", "none");
                    selectToType.Style.Add("display", "none");
                }
                txtFrom.Text = eventEntity.FromDay.ToString("MM/dd/yyyy");
                txtFromTime.Text = eventEntity.FromTime;
                selectFromType.SelectedValue = eventEntity.FromTimeType.ToString();
                txtTo.Text = eventEntity.ToDay.ToString("MM/dd/yyyy");
                txtToTime.Text = eventEntity.ToTime;
                selectToType.SelectedValue = eventEntity.ToTimeType.ToString();
                ddlAlert.SelectedValue = ((int)eventEntity.Alert).ToString();
                imgIcon.ImageUrl = eventEntity.IconPath;
                projApp.GetUserProjects(UserInfo).BindDropdown<ProjectDetailDTO>(ddlProjects, "Title", "ProjectID", "Please select...", "-1");
                ddlProjects.SelectedValue = eventEntity.ProjectID.ToString();
            }
            else
            {
                if (chkAllDay.Checked)
                {
                    txtFromTime.Style.Add("display", "none");
                    selectFromType.Style.Add("display", "none");
                    txtToTime.Style.Add("display", "none");
                    selectToType.Style.Add("display", "none");
                }
                else
                {
                    txtFromTime.Style.Add("display", "");
                    selectFromType.Style.Add("display", "");
                    txtToTime.Style.Add("display", "");
                    selectToType.Style.Add("display", "");
                }

            }
        }

        private EventsView ConstructEventsView(EventEntity eventEntity)
        {
            EventsView eventsView = new EventsView();
            eventsView.Alert = (AlertType)int.Parse(ddlAlert.SelectedItem.Value);
            eventsView.AllDay = chkAllDay.Checked;
            eventsView.CreatedBy = UserInfo.UserID;
            eventsView.CreatedOn = DateTime.Now;
            eventsView.Details = txtDetails.Text.NoHTML();
            //model.FamilyID = FamilyID;
            eventsView.FromDay = DateTime.Parse(txtFrom.Text);
            eventsView.FromTime = txtFromTime.Text;
            eventsView.FromTimeType = int.Parse(selectFromType.SelectedValue);
            if (!string.IsNullOrEmpty(Icon.Value))
            {
                eventsView.Icon = int.Parse(Icon.Value);
            }
            else
            {
                eventsView.Icon = eventEntity.Icon;
            }

            eventsView.Name = txtName.Text.NoHTML();
            eventsView.ToDay = DateTime.Parse(txtTo.Text);
            eventsView.ToTime = txtToTime.Text;
            eventsView.ToTimeType = int.Parse(selectToType.SelectedValue);
            eventsView.RoleIDs = ((int)Privates.OnlyMe).ToString();
            eventsView.Where = txtWhere.Text.NoHTML();
            eventsView.ID = eventEntity.ID;

            eventsView.CreatedOn = eventEntity.CreatedOn;
            eventsView.CreatedBy = eventEntity.CreatedBy;
            eventsView.UpdatedOn = eventEntity.UpdatedOn;
            eventsView.GroupID = eventEntity.GroupID;
            eventsView.HasAlert = eventEntity.HasAlert;
            eventsView.HasInvite = eventEntity.HasInvite;
            eventsView.Highlight = eventEntity.Highlight;
            eventsView.Privacy = eventEntity.Privacy;
            eventsView.ProjectID = int.Parse(ddlProjects.SelectedValue);
            return eventsView;
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            int id = QS("ID", 0);
            EventEntity eventEntity = new EventsApplication().GetEventInfo(id);
            EventsView model = ConstructEventsView(eventEntity);
            if (eventEntity.CreatedBy != UserInfo.UserID) return;
            model.CreatedBy = UserInfo.UserID;
            //model.FamilyID = familyID;
            //if (new EventsApplication().UpdateEvent(model, eventEntity))
            //{
            //    imgIcon.ImageUrl = model.IconPath;
            //    ShowSuccessMessageToClient("The change has been saved.", true, true, "350", "50");
            //}
            //else
            //{
            //    ShowFailMessageToClient("Edit event fail.", "350", "50");
            //}
        }

        public void btnDelete_Click(object sender, EventArgs e)
        {
            int id = QS("ID", 0);
            EventEntity entity = new EventsApplication().GetEventInfo(id);
            if (entity == null || entity.CreatedBy != UserInfo.UserID)
            {
                return;
            }

            if (new EventsApplication().Delete(id))
            {
                ShowSuccessMessageToClient("The event has been deleted.", true, true, "350", "50");
            }
            else
            {
                ShowFailMessageToClient("Delete fail.", "350", "50");
            }


        }
    }
}
