using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.SchedulesModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.App;
using System.Text;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class EditSchedules : BaseWebsitePage
    {

        /// <summary>
        ///  0 不可读，1可读， 2可编辑
        /// </summary>
        protected int Option
        {
            get { return ViewState["OPTION"] == null ? 0 : int.Parse(ViewState["OPTION"].ToString()); }
            set { ViewState["OPTION"] = value; }
        }

        //protected string jscheduleResult = string.Empty;

        public List<ScheduleTimeEntity> ScheduleTimeList
        {
            get
            {
                return ViewState["ScheduleTimeList"] == null ? new List<ScheduleTimeEntity>()
                    : (List<ScheduleTimeEntity>)ViewState["ScheduleTimeList"];
            }
            set { ViewState["ScheduleTimeList"] = value; }
        }

        private int targetUserId = 0;

        protected DateTime date;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (QS("ID", 0) > 0 && QS("target", 0) > 0)
                {
                    targetUserId = QS("target", 0);
                    SchedulesEntity entity = new App.SchedulesApplication().GetInfo(QS("ID", 0));
                    if (entity == null)
                    {
                        ShowFailMessageToClient("unauthorized access.");
                        return;
                    }
                    else if (entity.CreateBy == UserInfo.UserID) //可编辑 是当前用户创建的
                    {
                        Option = 2;
                    }
                    else if (entity.UserID == UserInfo.UserID) //该计划是当前者相关的，如Meeting
                        Option = 1;
                    else
                    {
                        List<UsersEntity> user = new App.SchedulesApplication().GetMeetingUsers(entity.MeetingID);
                        UsersEntity UsersEntity = new App.SchedulesApplication().GetMeetingUsers(entity.MeetingID).Find(r => r.Office == "CN");
                        bool isMeetingHasCNmemeber = UsersEntity != null;

                        if (UserInfo.Office == "US" || (UserInfo.Office == "CN" && isMeetingHasCNmemeber)) //US的人员可看所有的
                        {
                            Option = 1;
                        }
                        else  //CN 的人员只能看 CN 的
                        {
                            UsersEntity tmpUsersEntity = new App.UserApplication().GetUser(entity.CreateBy);
                            if (tmpUsersEntity.Office == "CN") //可看
                            {
                                Option = 1;
                            }
                            else
                            {
                                ShowFailMessageToClient("unauthorized access.");
                                return;
                            }
                        }
                    }
                    BindDrop(entity);
                    BindData(entity);
                    hdID.Value = entity.ID.ToString();
                    date = entity.PlanDate;
                    // BindGant();
                }
                else
                {
                    ShowFailMessageToClient("unauthorized access.");
                    return;
                }
                if (chkMeeting.Checked)
                {
                    trMeetingUsers.Visible = true;
                }
                else
                {
                    trMeetingUsers.Visible = false;
                }
            }
        }

        //private void BindGant()
        //{
        //    if (QS("ID", 0) > 0 && QS("target", 0) > 0)
        //    {
        //        targetUserId = QS("target", 0);
        //        SchedulesEntity entity = new App.SchedulesApplication().GetInfo(QS("ID", 0));
        //        List<SchedulesEntity> list = new App.SchedulesApplication().GetSchedules(entity.PlanDate, targetUserId);
        //        jscheduleResult = SerializeScheduleList(list);
        //    }
        //}

        private void BindData(SchedulesEntity entity)
        {
            txtTitle.Text = entity.Title;
            txtDescription.Text = entity.Description;
            ddlStartHours.SelectedValue = entity.StartTime.Split(':')[0];
            ddlStartMinute.SelectedValue = entity.StartTime.Split(':')[1];

            ddlEndHours.SelectedValue = entity.EndTime.Split(':')[0];
            ddlEndMinute.SelectedValue = entity.EndTime.Split(':')[1];

            if (entity.MeetingID != "") //meeting
            {
                chkMeeting.Enabled = false;
                if (entity.CreateBy == UserInfo.UserID && targetUserId == UserInfo.UserID) //如果是会议的创建者
                {
                    if (entity.MeetingStatus == 1) //等待审核
                    {
                        lstUsers.Enabled = true;
                        lstMeetingUsers.Enabled = true;
                        btnAddUsers.Visible = true;
                        btnMoveUsers.Visible = true;
                        btnDelete.Visible = true;
                    }
                    else if (entity.MeetingStatus == 2 || entity.MeetingStatus == 3) //同意了，或者取消了
                    {
                        Option = 1;
                        litAgree.Visible = true;
                        litAgree.Text = entity.MeetingStatus == 2 ? "agree" : "cancel";
                        //btnOK.Visible = false;
                        //btnNo.Visible = false;
                    }
                }
                else
                {
                    if (entity.MeetingStatus == 1) //等等审核
                    {
                        List<UsersEntity> meetingUsers = new App.SchedulesApplication()
                            .GetMeetingUsers(entity.MeetingID);
                        if (meetingUsers.Find(r => r.UserID == UserInfo.UserID) != null && targetUserId == UserInfo.UserID)
                        {
                            btnOK.Visible = true;
                            btnNo.Visible = true;
                        }
                    }
                    else
                    {
                        litAgree.Visible = true;
                        litAgree.Text = entity.MeetingStatus == 2 ? "agree" : "cancel";
                    }
                }

                chkMeeting.Checked = true;
                List<UsersEntity> meetingUsersList = new App.SchedulesApplication().GetMeetingUsers(entity.MeetingID);
                lstMeetingUsers.DataSource = meetingUsersList.OrderBy(r => r.FirstName);
                lstMeetingUsers.DataBind();

                List<UsersEntity> usersList = new App.UserApplication().ClonedActiveUserList.FindAll(r => r.Role != RolesEnum.CLIENT);

                foreach (UsersEntity tmpUses in meetingUsersList)
                {
                    UsersEntity findUser = usersList.Find(r => r.UserID == tmpUses.UserID);
                    if (findUser != null)
                        usersList.Remove(findUser);
                }
                lstUsers.DataSource = usersList;
                lstUsers.DataBind();

            }
            else //不是会议时
            {
                chkMeeting.Enabled = false;
                if (entity.CreateBy == UserInfo.UserID && targetUserId == UserInfo.UserID && entity.PlanDate >= DateTime.Now.Date)
                {
                    btnDelete.Visible = true;
                }

            }

            if (Option == 2) //过期的计划，只能读
            {
                if (entity.PlanDate < DateTime.Now.Date)
                    Option = 1;
            }

            switch (Option)
            {
                case 0: //不可访问
                case 1://可读
                    {
                        btnSave.Visible = false;
                        btnSaveAndNew.Visible = false;
                        break;
                    }
                case 2://可编辑的
                    {
                        btnSave.Visible = true;
                        btnSaveAndNew.Visible = true;
                        break;
                    }
            }

            if (entity.PlanDate < DateTime.Now.Date)
            {
                btnDelete.Visible = false;
            }
        }

        private void BindDrop(SchedulesEntity entity)
        {
            for (int i = 8; i < 24; i++)
            {
                ddlStartHours.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlEndHours.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlStartHours.SelectedValue = "9";
                ddlEndHours.SelectedValue = "9";
            }
            List<SchedulesEntity> list = new App.SchedulesApplication().GetSchedules(entity.PlanDate, targetUserId);

            ScheduleTimeList = new ScheduleTimeHelpers(list).Times;
            rptPlan.DataSource = ScheduleTimeList;
            rptPlan.DataBind();


            List<UsersEntity> UsersEntities = new App.UserApplication().GetActiveUserList().FindAll(r => r.RoleID != (int)RolesEnum.CLIENT);
            if (UsersEntities.Count > 0)
            {
                lstUsers.DataSource = UsersEntities.OrderBy(r => r.FirstName);
            }
            else
            {
                lstUsers.DataSource = UsersEntities;
            }
            lstUsers.DataBind();
        }

        protected void chkMeeting_CheckedChanged(object sender, EventArgs e)
        {
            lstUsers.Enabled = chkMeeting.Checked;
            lstMeetingUsers.Enabled = chkMeeting.Checked;
            btnAddUsers.Visible = chkMeeting.Checked;
            btnMoveUsers.Visible = chkMeeting.Checked;
            if (chkMeeting.Checked)
            {
                trMeetingUsers.Visible = true;
            }
            else
            {
                trMeetingUsers.Visible = false;
            }
            // BindGant();
        }

        protected void btnAddUsers_Click(object sender, EventArgs e)
        {
            ListBox lst = new ListBox();
            foreach (ListItem li in lstUsers.Items)
            {
                if (li.Selected)
                {
                    li.Selected = false;
                    lstMeetingUsers.Items.Add(li);
                }
                else
                    lst.Items.Add(li);
            }
            lstUsers.Items.Clear();
            foreach (ListItem li in lst.Items)
                lstUsers.Items.Add(li);
            // BindGant();
        }

        protected void btnMoveUsers_Click(object sender, EventArgs e)
        {
            ListBox lst = new ListBox();
            foreach (ListItem li in lstMeetingUsers.Items)
            {
                if (li.Selected)
                {
                    li.Selected = false;
                    lstUsers.Items.Add(li);
                }
                else
                    lst.Items.Add(li);
            }
            lstMeetingUsers.Items.Clear();
            foreach (ListItem li in lst.Items)
                lstMeetingUsers.Items.Add(li);
            // BindGant();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //当前用户为创建用户且meeting状态没有approve
            targetUserId = QS("target", 0);
            SchedulesEntity entity = new App.SchedulesApplication().GetInfo(int.Parse(hdID.Value));
            if (entity.CreateBy == UserInfo.UserID && (entity.MeetingStatus == 1 || string.IsNullOrEmpty(entity.MeetingID)) && entity.PlanDate >= DateTime.Now.Date)
            {
                SchedulesApplication schedulesApplication = new SchedulesApplication();
                if (string.IsNullOrEmpty(entity.MeetingID))
                {
                    if (schedulesApplication.Delete(entity.ID))
                    {
                        ShowSuccessMessageToClient(true, true);
                    }
                    else
                    {
                        ShowFailMessageToClient();
                    }
                }
                else
                {
                    if (schedulesApplication.DeleteMeetingSchedule(entity.MeetingID))
                    {
                        if (schedulesApplication.VoteMeeting(entity, DateTime.Now.Date, UserInfo))
                        {
                            ShowSuccessMessageToClient(true, true);
                        }
                        else
                        {
                            ShowFailMessageToClient();
                        }
                    }

                }
            }

            //List<SchedulesEntity> list = new App.SchedulesApplication().GetSchedules(entity.PlanDate, targetUserId);
            //jscheduleResult = SerializeScheduleList(list);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save(false);
            // BindGant();
        }

        protected void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            Save(true);
            // BindGant();
        }

        private void Save(bool isNew)
        {
            SchedulesEntity entity = new App.SchedulesApplication().GetInfo(int.Parse(hdID.Value));
            entity.Title = txtTitle.Text;
            entity.Description = txtDescription.Text;
            entity.StartTime = string.Format("{0}:{1}", ddlStartHours.SelectedValue, ddlStartMinute.SelectedValue);
            entity.EndTime = string.Format("{0}:{1}", ddlEndHours.SelectedValue, ddlEndMinute.SelectedValue);
            entity.UpdateBy = UserInfo.UserID;
            entity.UpdateOn = DateTime.Now;

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
                ScheduleTimeEntity t = ScheduleTimeList.Find(r => r.Cell == i && r.IsPlan && r.ScheduleID != entity.ID);
                if (t != null)
                {
                    ShowMessageToClient("The hours of the two schedules conflict.", 0, false, false);
                    return;
                }
            }

            if (chkMeeting.Checked)
            {
                List<UsersEntity> usersList = new List<UsersEntity>();
                foreach (ListItem li in lstMeetingUsers.Items)
                {
                    usersList.Add(new App.UserApplication().GetUser(int.Parse(li.Value)));
                }
                if (usersList.Find(r => r.UserID == UserInfo.UserID) == null) //如果不包括创建者，就添加他
                {
                    usersList.Add(UserInfo);
                }

                List<UsersEntity> moveSales = new App.SchedulesApplication().GetMeetingUsers(entity.MeetingID);
                List<UsersEntity> newList = new List<UsersEntity>();
                UsersEntity tmpUser;

                foreach (UsersEntity tmp in usersList)
                {
                    tmpUser = moveSales.Find(r => r.UserID == tmp.UserID);
                    if (tmpUser != null)
                        moveSales.Remove(tmpUser);
                    else
                        newList.Add(tmp);
                }

                if (new App.SchedulesApplication().Update(entity, moveSales, newList))
                {
                    if (isNew)
                        Server.Transfer("AddSchedules.aspx?Date=" + entity.PlanDate.ToString("MM/dd/yyyy"));
                    else
                        ShowSuccessMessageToClient(true, true);
                }
                else
                    ShowFailMessageToClient();
            }
            else
            {
                if (new App.SchedulesApplication().Update(entity))
                {
                    if (isNew)
                        Server.Transfer("AddSchedules.aspx?Date=" + entity.PlanDate.ToString("MM/dd/yyyy"));
                    else
                        ShowSuccessMessageToClient(true, true);
                }
                else
                    ShowFailMessageToClient();
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            SchedulesEntity entity = new App.SchedulesApplication().GetInfo(int.Parse(hdID.Value));
            if (entity != null && entity.UserID == UserInfo.UserID)
            {
                if (new App.SchedulesApplication().AgreeMeeting(entity, DateTime.Now.Date, UserInfo))
                    Response.Redirect(string.Format("EditSchedules.aspx?id={0}&target={1}", hdID.Value, UserInfo.UserID));
                else
                    ShowFailMessageToClient();
            }
            else
                ShowFailMessageToClient("unauthorized access.");
            // BindGant();
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            SchedulesEntity entity = new App.SchedulesApplication().GetInfo(int.Parse(hdID.Value));
            if (entity != null && entity.UserID == UserInfo.UserID)
            {
                if (new App.SchedulesApplication().VoteMeeting(entity, DateTime.Now.Date, UserInfo))
                    Response.Redirect(string.Format("EditSchedules.aspx?id={0}&target={1}", hdID.Value, UserInfo.UserID));
                else
                    ShowFailMessageToClient();
            }
            else
                ShowFailMessageToClient("unauthorized access.");
            // BindGant();
        }
    }
}
