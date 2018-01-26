using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.SchedulesModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class AddSchedules : BaseWebsitePage
    {
        protected DateTime Date
        {
            get { return ViewState["Date"] == null ? DateTime.Now.Date : DateTime.Parse(ViewState["Date"].ToString()); }
            set { ViewState["Date"] = value; }
        }

        public List<ScheduleTimeEntity> ScheduleTimeList
        {
            get
            {
                return ViewState["ScheduleTimeList"] == null ? new List<ScheduleTimeEntity>()
                    : (List<ScheduleTimeEntity>)ViewState["ScheduleTimeList"];
            }
            set { ViewState["ScheduleTimeList"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Date"] != null)
                {
                    DateTime tmpDate;
                    if (DateTime.TryParse(QS("date"), out tmpDate))
                    {
                        Date = tmpDate;
                        if (Date < DateTime.Now.Date)
                        {
                            ShowFailMessageToClient("unauthorized access.");
                            return;
                        }
                    }
                    else
                    {
                        ShowFailMessageToClient("unauthorized access.");
                        return;
                    }
                }
                else
                {
                    Date = DateTime.Now.Date;
                }
                BindDrop();
                trMeetingUsers.Style.Add("display", "none");
                // BindGant();
            }
        }

        //private void BindGant()
        //{
        //    List<SchedulesEntity> list = new App.SchedulesApplication().GetSchedules(DateTime.Parse(QS("Date")), UserInfo.UserID);
        //    jscheduleResult = SerializeScheduleList(list);
        //}

        private void BindDrop()
        {
            for (int i = 8; i < 24; i++)
            {
                ddlStartHours.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlEndHours.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlStartHours.SelectedValue = "9";
                ddlEndHours.SelectedValue = "9";
            }

            List<SchedulesEntity> list = new App.SchedulesApplication().GetSchedules(Date, UserInfo.UserID);

            ScheduleTimeList = new ScheduleTimeHelpers(list).Times;
            rptPlan.DataSource = ScheduleTimeList;
            rptPlan.DataBind();
            List<UsersEntity> UsersEntities = new App.UserApplication().GetActiveUserList().FindAll(r => r.RoleID != (int)RolesEnum.CLIENT && r.UserID != UserInfo.UserID);
            if (UsersEntities.Count > 0)
            {
                lstUsers.DataSource = UsersEntities.OrderBy(r => r.FirstName);
            }
            else
            {
                lstUsers.DataSource = UsersEntities;
            }
            lstUsers.DataBind();
            lstMeetingUsers.Items.Add(new ListItem(string.Format("{0}",
                                    UserInfo.FirstAndLastName), UserInfo.ID.ToString()));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save(false);
        }

        protected void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            Save(true);
        }

        private void Save(bool isNew)
        {
            SchedulesEntity entity = new SchedulesEntity();
            entity.Title = txtTitle.Text;
            entity.Description = txtDescription.Text;
            entity.CreateBy = UserInfo.UserID;
            entity.CreateOn = DateTime.Now;
            entity.StartTime = string.Format("{0}:{1}", ddlStartHours.SelectedValue, ddlStartMinute.SelectedValue);
            entity.EndTime = string.Format("{0}:{1}", ddlEndHours.SelectedValue, ddlEndMinute.SelectedValue);
            entity.UpdateBy = UserInfo.UserID;
            entity.UpdateOn = DateTime.Now;
            entity.PlanDate = Date;

            int start = ScheduleTimeHelpers.TimeHandle(entity.StartTime);
            int end = ScheduleTimeHelpers.TimeHandle(entity.EndTime);

            if (start > end)
            {
                ShowMessageToClient("Please entry again , Starting time should not exceed finishing time.", 0, false, false);
                return;
            }

            if (DateTime.Parse(string.Format("{0} {1}", entity.PlanDate.ToString("MM/dd/yyyy"), entity.StartTime)) < DateTime.Now)
            {
                ShowMessageToClient("Plan time not less than the current time.", 0, false, false);
                return;
            }

            for (int i = start; i < end; i++)
            {
                ScheduleTimeEntity t = ScheduleTimeList.Find(r => r.Cell == i && r.IsPlan);
                if (t != null)
                {
                    ShowMessageToClient("The hours of the two schedules conflict.", 0, false, false);
                    return;
                }
            }

            if (chkMeeting.Checked)
            {
                entity.MeetingID = string.Format("{0}:{1}", UserInfo.UserID, DateTime.Now.ToString("MM_dd_yy_HH:mm_ss"));
                entity.MeetingStatus = 1;
                List<UsersEntity> usersList = new List<UsersEntity>();
                foreach (ListItem li in lstMeetingUsers.Items)
                {
                    usersList.Add(new App.UserApplication().GetUser(int.Parse(li.Value)));
                }
                if (usersList.Find(r => r.UserID == UserInfo.UserID) == null) //如果不包括创建者，就添加他
                {
                    usersList.Add(UserInfo);
                }
                if (new App.SchedulesApplication().Add(entity, usersList, UserInfo) > 0)
                {

                    if (isNew)
                        Redirect("AddSchedules.aspx?Date=" + Date.ToString("MM/dd/yyyy"));
                    else
                        ShowSuccessMessageToClient(true, true);
                }
                else
                    ShowFailMessageToClient();

            }
            else
            {
                entity.MeetingID = string.Empty;
                entity.UserID = UserInfo.UserID;

                if (new App.SchedulesApplication().Add(entity) > 0)
                {
                    //List<SchedulesEntity> list = new App.SchedulesApplication().GetSchedules(entity.PlanDate, UserInfo.UserID);
                    //jscheduleResult = SerializeScheduleList(list);

                    if (isNew)
                        Redirect("AddSchedules.aspx?Date=" + Date.ToString("MM/dd/yyyy"));
                    else
                        ShowSuccessMessageToClient(true, true);
                }
                else
                    ShowFailMessageToClient();
            }
        }
    }
}
