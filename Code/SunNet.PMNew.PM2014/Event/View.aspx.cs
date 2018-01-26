using SunNet.PMNew.App;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProjectModel;
using System.Data;
using SunNet.PMNew.Entity.UserModel;
using System.Configuration;
using SunNet.PMNew.Framework;

namespace SunNet.PMNew.PM2014.Event
{
    public partial class View : BasePage
    {
        private ProjectApplication projApp;

        protected void Page_Load(object sender, EventArgs e)
        {
            projApp = new ProjectApplication();

            if (!Page.IsPostBack)
            {
                int id = QS("ID", 0);
                int canEdit = QS("c", 0);
                EventEntity eventEntity = new EventsApplication().GetEventInfo(id);
                ltrlName.Text = eventEntity.Name;
                ltrlDetail.Text = eventEntity.Details;
                ltrlWhere.Text = eventEntity.Where;
                chkAllDay.Checked = eventEntity.AllDay;
                chkAllDay.Enabled = false;
                string fromTime = string.Empty;
                string toTime = string.Empty;

                if (!chkAllDay.Checked)
                {
                    fromTime = eventEntity.FromDayString + " " + eventEntity.FromTime + " " + (eventEntity.FromTimeType == 1 ? "AM" : "PM");
                    toTime = eventEntity.ToDayString + " " + eventEntity.ToTime + " " + (eventEntity.ToTimeType == 1 ? "AM" : "PM");
                }
                else
                {
                    fromTime = eventEntity.FromDayString;
                    toTime = eventEntity.ToDayString;
                }
                ltrlFrom.Text = fromTime;
                ltrlTo.Text = toTime;
                ltrlAlert.Text = eventEntity.Alert.GetName<AlertType>();
                imgIcon.ImageUrl = eventEntity.IconPath;
                if (eventEntity.ProjectID != -1)
                {
                    ProjectsEntity projectsEntity = projApp.Get(eventEntity.ProjectID);
                    ltrlProject.Text = projectsEntity.Title;
                }
                else
                {
                    ltrlProject.Text = string.Empty;
                }
                litCreated.Text = new App.UserApplication().GetUser(eventEntity.CreatedBy).GetClientUserName(UserInfo);

                List<EventInviteEntity> list = new App.EventsApplication().GetEventInvites(eventEntity.ID);
                UsersEntity userEntity;
                foreach (EventInviteEntity tmpItem in list)
                {
                    if (tmpItem.UserID > 0)
                    {
                        userEntity = new App.UserApplication().GetUser(tmpItem.UserID);
                        tmpItem.FirstName = userEntity.FirstName;
                        tmpItem.LastName = userEntity.LastName;
                    }
                }

                if (eventEntity.ProjectID.ToString() == Config.HRProjectID)
                {
                    chkOff.Checked = eventEntity.IsOff;
                }
                else
                {
                    div_off.Attributes.Add("style", "display:none");
                }

                rptInvite.DataSource = list.OrderBy(r => r.LastName);
                rptInvite.DataBind();
            }
            ((Pop)(this.Master)).Width = 425;
        }
    }
}