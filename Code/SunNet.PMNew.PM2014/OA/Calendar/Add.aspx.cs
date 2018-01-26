using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Entity.SchedulesModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using System.Web.UI.HtmlControls;


namespace SunNet.PMNew.PM2014.OA.Calendar
{
    public partial class Add : BasePage
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
                divUser.Style.Add("display", "none");
            }
            ((Pop)(this.Master)).Width = 600;
        }


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
                rtpUser.DataSource = UsersEntities.OrderBy(r => r.FirstName);
            }
            else
            {
                rtpUser.DataSource = UsersEntities;
            }
            rtpUser.DataBind();
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

            if (string.IsNullOrEmpty(entity.Title))
            {
                ShowFailMessageToClient("Please enter the title.");
                return;
            }
            if (start > end)
            {
                ShowFailMessageToClient("Please entry again , Starting time should not exceed finishing time.");
                return;
            }

            if (DateTime.Parse(string.Format("{0} {1}", entity.PlanDate.ToString("MM/dd/yyyy"), entity.StartTime)) < DateTime.Now)
            {
                ShowFailMessageToClient("Plan time not less than the current time.");
                return;
            }

            for (int i = start; i < end; i++)
            {
                ScheduleTimeEntity t = ScheduleTimeList.Find(r => r.Cell == i && r.IsPlan);
                if (t != null)
                {
                    ShowFailMessageToClient("The hours of the two schedules conflict.");
                    return;
                }
            }

            if (chkMeeting.Checked)
            {
                entity.MeetingID = string.Format("{0}:{1}", UserInfo.UserID, DateTime.Now.ToString("MM_dd_yy_HH:mm_ss"));
                entity.MeetingStatus = 1;
                List<UsersEntity> usersList = new List<UsersEntity>();
                for (int i = 0; i < this.rtpUser.Items.Count; i++)
                {
                    CheckBox check = (CheckBox)this.rtpUser.Items[i].FindControl("cbUser");
                    if (check.Checked)
                    {
                        Literal lit = this.rtpUser.Items[i].FindControl("ltlid") as Literal;
                        usersList.Add(new App.UserApplication().GetUser(int.Parse(lit.Text)));
                    }
                }
                if (usersList.Find(r => r.UserID == UserInfo.UserID) == null) //如果不包括创建者，就添加他
                {
                    usersList.Add(UserInfo);
                }
                if (new App.SchedulesApplication().Add(entity, usersList, UserInfo) > 0)
                {
                    if (isNew)
                        Redirect("Add.aspx?Date=" + Date.ToString("MM/dd/yyyy"), true);
                    else
                        Redirect(EmptyPopPageUrl,false,true);
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
                    if (isNew)
                        Redirect("Add.aspx?Date=" + Date.ToString("MM/dd/yyyy"),true);
                    else
                        Redirect(EmptyPopPageUrl, false, true);
                }
                else
                    ShowFailMessageToClient();
            }
        }


    }
}