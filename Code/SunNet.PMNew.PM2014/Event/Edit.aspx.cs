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
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;
using System.Data;
using System.IO;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.CompanyModel;
using System.Configuration;
using SunNet.PMNew.PM2014.OA.Pto;

namespace SunNet.PMNew.PM2014.Event
{
    public partial class Edit : BasePage
    {
        private ProjectApplication projApp;
        protected bool OnlyRead = false;
        protected int Times;
        protected DateTime FromDay = DateTime.MinValue;//Event日期

        public bool NotEnoughPTOHour = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            projApp = new ProjectApplication();

            if (!Page.IsPostBack)
            {
                NotEnoughPTOHour = CurrentPTOHoursNotEnough();
                int id = QS("ID", 0);
                EventEntity eventEntity = new EventsApplication().GetEventInfo(id);
                if (eventEntity != null)
                {
                    FromDay = eventEntity.FromDay;
                    var projectEntity = new ProjectApplication().Get(eventEntity.ProjectID);
                    if (eventEntity.CreatedBy != UserInfo.UserID && projectEntity.Title != "0_PTO")
                    {
                        OnlyRead = true;
                        Response.Redirect("/Event/View.aspx?ID=" + eventEntity.ID);
                    }
                    var ptoAdmin = Config.PtoAdmin;
                    if (ptoAdmin == null)
                    {
                        if (projectEntity.Title == "0_PTO" && eventEntity.FromDay < DateTime.Now &&
                            eventEntity.CreatedBy == UserInfo.UserID)
                        {
                            OnlyRead = true;
                            Response.Redirect("/Event/View.aspx?ID=" + eventEntity.ID);
                        }
                    }
                    else
                    {
                        var ptoAdmins = ptoAdmin.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (!ptoAdmins.Contains(UserInfo.UserName))
                        {
                            if (projectEntity.Title == "0_PTO" && eventEntity.FromDay < DateTime.Now &&
                            eventEntity.CreatedBy == UserInfo.UserID)
                            {
                                OnlyRead = true;
                                Response.Redirect("/Event/View.aspx?ID=" + eventEntity.ID);
                            }
                            if (projectEntity.Title == "0_PTO" && eventEntity.CreatedBy != UserInfo.UserID)
                            {
                                OnlyRead = true;
                                Response.Redirect("/Event/View.aspx?ID=" + eventEntity.ID);
                            }
                        }
                    }

                    Times = eventEntity.Times;
                    txtName.Text = eventEntity.Name;
                    //txtName.Enabled = projectEntity.ProjectCode != "PTO";取消不能编辑pto
                    txtDetails.Text = eventEntity.Details;
                    txtWhere.Text = eventEntity.Where;
                    chkAllDay.Checked = eventEntity.AllDay;
                    if (chkAllDay.Checked)
                    {
                        txtFromTime.Style.Add("display", "none");
                        txtToTime.Style.Add("display", "none");
                    }
                    txtFrom.Enabled = eventEntity.FromDay >= DateTime.Today ? true : false;
                    txtFrom.Text = eventEntity.FromDay.ToString("MM/dd/yyyy");
                    TxtFromHide.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    txtFromTime.Text = eventEntity.FromTime + (eventEntity.FromTimeType == 1 ? "am" : "pm");
                    txtTo.Enabled = eventEntity.ToDay >= DateTime.Today ? true : false;
                    txtTo.Text = eventEntity.ToDay.ToString("MM/dd/yyyy");
                    txtToTime.Text = eventEntity.ToTime + (eventEntity.ToTimeType == 1 ? "am" : "pm");
                    ddlAlert.SelectedValue = ((int)eventEntity.Alert).ToString();
                    imgIcon.ImageUrl = eventEntity.IconPath;
                    List<ProjectDetailDTO> projectList = projApp.GetUserProjectsForCreateObject(UserInfo, eventEntity.ProjectID);
                    switch (UserInfo.Role)
                    {
                        case RolesEnum.Leader:
                        case RolesEnum.DEV:
                        case RolesEnum.QA:
                            projectList = projectList.FindAll(r => r.CompanyID == Config.SunnetCompany);
                            break;
                    }
                    List<EventInviteEntity> list = GetInviteData(eventEntity);

                    rptInviteUser.DataSource = list.FindAll(r => r.UserID > 0).OrderBy(r => r.LastName);
                    rptInviteUser.DataBind();
                    if (rptInviteUser.Items.Count == 0)
                    {
                        rptInviteUser.Visible = false;
                        litNoUser.Text = " <li>No Users</li>";
                    }
                    if (eventEntity.ProjectID.ToString() == Config.HRProjectID)
                    {
                        chkOff.Checked = eventEntity.IsOff;
                    }
                    else
                    {
                        div_off.Attributes.Add("style", "display:none");
                    }

                    rptOtherUser.DataSource = list.FindAll(r => r.UserID == 0).OrderBy(r => r.FirstName).OrderBy(r => r.LastName);
                    rptOtherUser.DataBind();

                }
            }
            else
            {
                if (chkAllDay.Checked)
                {
                    txtFromTime.Style.Add("display", "none");
                    txtToTime.Style.Add("display", "none");
                }
                else
                {
                    txtFromTime.Style.Add("display", "");
                    txtToTime.Style.Add("display", "");
                }

            }
            ((Pop)(this.Master)).Width = 780;
        }

        private List<EventInviteEntity> GetInviteData(EventEntity eventEntity, bool isDelete = false)
        {
            List<EventInviteEntity> list = new App.EventsApplication().GetEventInvites(eventEntity.ID);
            foreach (EventInviteEntity tmpItem in list.FindAll(r => r.UserID > 0))
            {
                UsersEntity tmpUser = new App.UserApplication().GetUser(tmpItem.UserID);
                CompanysEntity comanyEntity = new App.CompanyApplication().GetCompany(tmpUser.CompanyID);
                tmpItem.FirstName = tmpUser.FirstName;
                tmpItem.LastName = tmpUser.LastName;
                tmpItem.Email = tmpUser.Email;
                tmpItem.IsSeleted = true;
                tmpItem.Title = tmpUser.Title;
                tmpItem.CompanyName = comanyEntity.CompanyName;
            }
            if (isDelete)
                return list;

            List<int> listUserId = new App.ProjectApplication().GetActiveUserIdByProjectId(eventEntity.ProjectID);
            ProjectsEntity projectEntity = new App.ProjectApplication().Get(eventEntity.ProjectID);
            litProject.Text = projectEntity.Title;

            listUserId.Remove(UserInfo.UserID);
            if (UserInfo.Office == "CN" && UserInfo.UserType == "SUNNET")//山诺 上海的员工只能获取本公司的项目的相关人员
            {
                if (projectEntity.CompanyID == Config.SunnetCompany)
                {
                    foreach (int tmpId in listUserId)
                    {
                        UsersEntity tmpUser = new App.UserApplication().GetUser(tmpId);
                        if (tmpUser == null) continue;
                        if (tmpUser.UserType == "SUNNET")
                        {
                            if (list.Find(r => r.UserID == tmpUser.UserID) == null)
                            {
                                list.Add(new EventInviteEntity() { EventID = eventEntity.ID, UserID = tmpUser.UserID, FirstName = tmpUser.FirstName, LastName = tmpUser.LastName });
                            }
                        }
                    }
                }
            }
            else
            {
                if (UserInfo.Role == RolesEnum.CLIENT || UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales || UserInfo.Role == RolesEnum.Contactor)
                {
                    foreach (int tmpId in listUserId)
                    {
                        UsersEntity tmpUser = new App.UserApplication().GetUser(tmpId);
                        if (tmpUser == null) continue;
                        CompanysEntity comanyEntity = new App.CompanyApplication().GetCompany(tmpUser.CompanyID);
                        if (projectEntity.CompanyID == Config.SunnetCompany) //如果是公司项目
                        {
                            if (list.Find(r => r.UserID == tmpUser.UserID) == null)
                            {
                                list.Add(new EventInviteEntity()
                                {
                                    EventID = eventEntity.ID,
                                    UserID = tmpUser.UserID
                                    ,
                                    FirstName = tmpUser.FirstName,
                                    LastName = tmpUser.LastName,
                                    Title = tmpUser.Title,
                                    CompanyName = comanyEntity.CompanyName
                                });
                            }
                        }
                        else
                        {
                            if (tmpUser.Role == RolesEnum.CLIENT || tmpUser.Role == RolesEnum.ADMIN || tmpUser.Role == RolesEnum.PM || tmpUser.Role == RolesEnum.Sales || UserInfo.Role == RolesEnum.Contactor)
                            {
                                if (list.Find(r => r.UserID == tmpUser.UserID) == null)
                                {
                                    list.Add(new EventInviteEntity()
                                    {
                                        EventID = eventEntity.ID,
                                        UserID = tmpUser.UserID
                                        ,
                                        FirstName = tmpUser.FirstName,
                                        LastName = tmpUser.LastName,
                                        Title = tmpUser.Title,
                                        CompanyName = comanyEntity.CompanyName
                                    });
                                }

                            }
                        }
                    }
                }
            }
            return list;
        }

        private EventsView ConstructEventsView(EventEntity eventEntity)
        {
            EventsView eventsView = new EventsView();
            eventsView.Alert = (AlertType)int.Parse(ddlAlert.SelectedItem.Value);
            eventsView.AllDay = chkAllDay.Checked;
            eventsView.CreatedBy = UserInfo.UserID;
            eventsView.CreatedOn = DateTime.Now;
            eventsView.Details = txtDetails.Text.Trim().NoHTML();
            eventsView.FromDay = DateTime.Parse(txtFrom.Text);

            string tmpFromTime = txtFromTime.Text.Trim().ToLower();
            if (tmpFromTime.EndsWith("am") || tmpFromTime.EndsWith("pm"))
            {
                eventsView.FromTime = tmpFromTime.Remove(tmpFromTime.Length - 2).Trim();
                eventsView.FromTimeType = tmpFromTime.Substring(tmpFromTime.Length - 2) == "am" ? 1 : 2;
            }
            else
            {
                eventsView.FromTime = txtFromTime.Text;
                eventsView.FromTimeType = 1;
            }
            if (!string.IsNullOrEmpty(Icon.Value))
            {
                eventsView.Icon = int.Parse(Icon.Value);
            }
            else
            {
                eventsView.Icon = eventEntity.Icon;
            }

            eventsView.Name = txtName.Text.Trim().NoHTML();
            eventsView.ToDay = DateTime.Parse(txtTo.Text);
            string tmpToime = txtToTime.Text.Trim().ToLower();
            if (tmpToime.EndsWith("am") || tmpToime.EndsWith("pm"))
            {
                eventsView.ToTime = tmpToime.Remove(tmpToime.Length - 2).Trim();
                eventsView.ToTimeType = tmpToime.Substring(tmpToime.Length - 2) == "am" ? 1 : 2;
            }
            else
            {
                eventsView.ToTime = txtFromTime.Text;
                eventsView.ToTimeType = 1;
            }
            eventsView.RoleIDs = ((int)Privates.OnlyMe).ToString();
            eventsView.Where = txtWhere.Text.Trim().NoHTML();
            eventsView.ID = eventEntity.ID;

            eventsView.CreatedOn = eventEntity.CreatedOn;
            eventsView.CreatedBy = eventEntity.CreatedBy;
            eventsView.UpdatedOn = eventEntity.UpdatedOn;
            eventsView.GroupID = eventEntity.GroupID;
            eventsView.HasAlert = eventEntity.HasAlert;
            eventsView.HasInvite = eventEntity.HasInvite;
            eventsView.Highlight = eventEntity.Highlight;
            eventsView.Privacy = eventEntity.Privacy;
            eventsView.ProjectID = eventEntity.ProjectID;
            eventsView.IsOff = chkOff.Checked;
            return eventsView;
        }

        private List<EventInviteEntity> BuilderInvite(EventsView model)
        {
            ///获取旧数据
            List<EventInviteEntity> oldlist = new App.EventsApplication().GetEventInvites(model.ID);
            foreach (EventInviteEntity tmpItem in oldlist)
            {
                if (tmpItem.UserID > 0)
                {
                    UsersEntity tmpUser = new App.UserApplication().GetUser(tmpItem.UserID);
                    tmpItem.FirstName = tmpUser.FirstName;
                    tmpItem.LastName = tmpUser.LastName;
                    tmpItem.Email = tmpUser.Email;
                }
            }


            ///获取project users 选中的用户，并区分是新加的，还是原本就有的
            List<EventInviteEntity> inviteList = new List<EventInviteEntity>();
            string projectUserIds = QF("chkProjectUser");
            if (projectUserIds.Trim() != string.Empty)
            {
                List<int> listUserId = new App.ProjectApplication().GetActiveUserIdByProjectId(model.ProjectID);
                projectUserIds = projectUserIds.Trim();
                if (projectUserIds.EndsWith(","))
                    projectUserIds = projectUserIds.Remove(projectUserIds.Length - 1);
                int tmpId;
                foreach (string s in projectUserIds.Split(','))
                {
                    if (int.TryParse(s, out tmpId))
                    {
                        if (listUserId.Contains(tmpId))
                        {
                            EventInviteEntity newEntity = new EventInviteEntity()
                            {
                                CreatedID = UserInfo.UserID,
                                EventID = model.ID,
                                FromDay = model.FromDay,
                                UserID = tmpId,
                                Email = "",
                                FirstName = "",
                                LastName = ""
                            };
                            if (oldlist.Find(r => r.UserID == tmpId) != null)
                                newEntity.OptionStatus = 1;
                            else
                                newEntity.OptionStatus = 2;
                            inviteList.Add(newEntity);
                        }
                    }
                }
            }

            ///标记被删除的project users
            foreach (EventInviteEntity item in oldlist.FindAll(r => r.UserID > 0))
            {
                if (inviteList.Find(r => r.UserID == item.UserID) == null)
                {
                    item.OptionStatus = 3;
                    inviteList.Add(item);
                }
            }

            ///获取other user,  如果有填写 Email 则区分是新加的，还是原有的
            int otherusers_count = QF("otherusers_count", 0);
            if (otherusers_count > 0)
            {
                string firstName;
                string lastName;
                string email;
                for (int i = 1; i <= otherusers_count; i++)
                {
                    firstName = QF("txtOtherUserFirst" + i).Trim();
                    lastName = QF("txtOtherUserLast" + i).Trim();
                    email = QF("txtOtherUserEmail" + i).Trim().ToLower();
                    if (firstName != string.Empty && lastName != string.Empty)
                    {
                        EventInviteEntity newEntity = new EventInviteEntity()
                        {
                            CreatedID = UserInfo.UserID,
                            LastName = lastName,
                            FirstName = firstName,
                            Email = email,
                            FromDay = model.FromDay,
                            EventID = model.ID,
                            UserID = 0
                        };

                        if (email != string.Empty)
                        {
                            if (oldlist.Find(r => r.Email.ToLower() == email) != null)
                                newEntity.OptionStatus = 1;
                            else
                                newEntity.OptionStatus = 2;
                        }
                        else
                            newEntity.OptionStatus = 2;
                        inviteList.Add(newEntity);
                    }
                }

                ///标记被删除的 other  users(只针对有email的)
                foreach (EventInviteEntity item in oldlist.FindAll(r => r.UserID == 0))
                {
                    if (item.Email != string.Empty)
                    {
                        if (inviteList.Find(r => r.Email.ToLower() == item.Email) == null)
                        {
                            item.OptionStatus = 3;
                            inviteList.Add(item);
                        }
                    }
                }
            }
            return inviteList;
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == string.Empty)
            {
                ShowFailMessageToClient("Please enter title.");
                return;
            }

            int id = QS("ID", 0);
            EventEntity eventEntity = new EventsApplication().GetEventInfo(id);
            EventsView model = ConstructEventsView(eventEntity);
            if (eventEntity.CreatedBy != UserInfo.UserID) return;

            model.CreatedBy = UserInfo.UserID;
            List<EventInviteEntity> inviteList = BuilderInvite(model);

            DateTime fromDay = model.FromDay;
            DateTime toDay = model.ToDay;
            model.ToDay = model.FromDay;

            if (new EventsApplication().UpdateEvent(model, eventEntity, BuilderInvite(model)))
            {
                if (!model.AllDay)
                    model.FromDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
                    , model.FromDay.Month, model.FromDay.Day, model.FromDay.Year, model.FromTime, model.FromTimeType == 1 ? "AM" : "PM"));

                if (model.FromDay != eventEntity.FromDay) //修改even时，有修改它的fromdate 时，要发邮件通知邀请的人
                {
                    SendEamil(inviteList, model, true);
                }
                else
                    SendEamil(inviteList, model, false);

                //如果改变了ToDay
                if (toDay > fromDay)
                {
                    new EventsApplication().Delete(eventEntity.ID, eventEntity.FromDay);
                    List<EventEntity> eventList = new List<EventEntity>();
                    model.ToDay = toDay;
                    model.Repeat = RepeatType.Every_Day;
                    model.End = EndType.on_date;
                    model.EndDate = model.ToDay;

                    new EventsApplication().AddEvents(model, inviteList, out eventList);
                }
                Redirect(Request.RawUrl, false, true);
            }
            else
            {
                ShowFailMessageToClient("Edit event fail.");
            }
        }

        /// <summary>
        /// 批量保存时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == string.Empty)
            {
                ShowFailMessageToClient("Please enter title.");
                return;
            }

            int id = QS("ID", 0);
            EventEntity eventEntity = new EventsApplication().GetEventInfo(id);
            EventsView model = ConstructEventsView(eventEntity);
            if (eventEntity.CreatedBy != UserInfo.UserID) return;

            model.CreatedBy = UserInfo.UserID;
            List<EventInviteEntity> inviteList = BuilderInvite(model);

            DataSet updatingEntities = new EventsApplication().GetUpdateAndDeleteEvents(eventEntity.CreatedBy, eventEntity.CreatedOn, eventEntity.FromDay);

            //如果改变了From ToDay
            if (model.ToDay > model.FromDay || model.FromDay != eventEntity.FromDay.Date)
            {
                //删除从选中的日期以后的相关Event
                if (new EventsApplication().DeleteAll(eventEntity.CreatedBy, eventEntity.CreatedOn, eventEntity.FromDay.Date))
                {
                    //删除Event所对应的TimeSheet
                    DeleteTimeSheets(updatingEntities);
                }
                else
                {
                    ShowFailMessageToClient("Edit event fail.");
                }
                List<EventEntity> eventList = new List<EventEntity>();
                model.Repeat = RepeatType.Every_Day;
                model.End = EndType.on_date;
                model.EndDate = model.ToDay;
                new EventsApplication().AddEvents(model, inviteList, out eventList);
                Redirect(Request.RawUrl, false, true);
            }

            DataRowCollection dr = null;
            if (updatingEntities != null)
            {
                if (updatingEntities.Tables.Count > 0)
                {
                    if (updatingEntities.Tables[0].Rows.Count > 0)
                    {
                        dr = updatingEntities.Tables[0].Rows;
                    }
                }
            }

            if (dr != null)  //循环更新成相同的Event
            {
                foreach (DataRow item in dr)
                {
                    List<EventInviteEntity> newInvite = inviteList; //每次都传递要修改的EventInvite集合
                    int eventId = int.Parse(item[0].ToString());
                    if (eventId != id)
                    {
                        EventEntity newEventEntity = new EventsApplication().GetEventInfo(eventId);
                        model.ID = eventId;
                        model.FromDay = newEventEntity.FromDay;  //FromDay和ToDay应保持原来的值
                        model.ToDay = newEventEntity.ToDay;
                        eventEntity.IsOff = newEventEntity.IsOff;//保持原来的Event的IsOff选项，用于处理Timesheets
                    }
                    if (new EventsApplication().UpdateEvent(model, eventEntity, newInvite))
                    {
                        if (!model.AllDay)
                            model.FromDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
                            , model.FromDay.Month, model.FromDay.Day, model.FromDay.Year, model.FromTime, model.FromTimeType == 1 ? "AM" : "PM"));

                        SendEamil(inviteList, model, false);
                    }
                    else
                    {
                        ShowFailMessageToClient("Edit event fail.");
                    }
                }
                Redirect(Request.RawUrl, false, true);
            }
            else
            {
                ShowFailMessageToClient("Edit event fail.");
            }
        }

        public void btnDelete_Click(object sender, EventArgs e)
        {
            int id = QS("ID", 0);
            EventEntity entity = new EventsApplication().GetEventInfo(id);
            if (entity == null || entity.CreatedBy != UserInfo.UserID)
            {
                return;
            }

            List<EventInviteEntity> list = GetInviteData(entity, true);
            if (new EventsApplication().Delete(id, entity.FromDay.Date))
            {
                EventsView eventView = new EventsView();
                eventView.Name = entity.Name;
                eventView.Where = entity.Where;
                eventView.Details = entity.Details;
                eventView.ProjectID = entity.ProjectID;
                eventView.FromDay = entity.FromDay;
                eventView.FromTime = entity.FromTime;
                eventView.FromTimeType = entity.FromTimeType;
                eventView.ToDay = entity.ToDay;
                eventView.ToTime = entity.ToTime;
                eventView.ToTimeType = entity.ToTimeType;
                eventView.AllDay = entity.AllDay;


                SendEamil(list, eventView, false, true);
                Redirect(Request.RawUrl, false, true);
            }
            else
            {
                ShowFailMessageToClient("Delete fail.");
            }
        }

        public void btnDeleteAll_Click(object sender, EventArgs e)
        {
            int id = QS("ID", 0);
            EventEntity entity = new EventsApplication().GetEventInfo(id);
            if (entity == null || entity.CreatedBy != UserInfo.UserID)
            {
                return;
            }
            DataSet updatingEntities = new EventsApplication().GetUpdateAndDeleteEvents(entity.CreatedBy, entity.CreatedOn, entity.FromDay);
            if (new EventsApplication().DeleteAll(entity.CreatedBy, entity.CreatedOn, entity.FromDay.Date))
            {
                //删除Event所对应的TimeSheet
                DeleteTimeSheets(updatingEntities);
                Redirect(EmptyPopPageUrl, false, true);
            }
            else
            {
                ShowFailMessageToClient("Delete fail.");
            }
        }

        private void DeleteTimeSheets(DataSet deleteEntities)
        {
            //删除Event所对应的TimeSheet
            DataRowCollection dr = null;
            if (deleteEntities != null)
            {
                if (deleteEntities.Tables.Count > 0)
                {
                    if (deleteEntities.Tables[0].Rows.Count > 0)
                    {
                        dr = deleteEntities.Tables[0].Rows;
                    }
                }
            }
            if (dr != null)
            {
                foreach (DataRow item in dr)
                {
                    int eventId = int.Parse(item[0].ToString());
                    DateTime fromDay = DateTime.Parse(item[1].ToString()).Date;
                    new EventsApplication().DeleteTimeSheet(eventId, fromDay);
                }
            }
        }

        public string GetEmailExecuter(string fileName)
        {
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Template\\event\\" + fileName;
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                return "";
            }
        }

        protected void SendEamil(List<EventInviteEntity> inviteList, EventsView model, bool isEditDate, bool isDelete = false)
        {
            string AddContent = GetEmailExecuter("CreateEvent.txt").Replace("[Host]", UserInfo.FirstAndLastName);
            string RemoveUserContent = GetEmailExecuter("RemoveUser.txt").Replace("[Host]", UserInfo.FirstAndLastName);
            string UpdateContent = GetEmailExecuter("EventUpdateTime.txt").Replace("[Host]", UserInfo.FirstAndLastName);
            string DeleteContent = GetEmailExecuter("DeletedEvent.txt").Replace("[Host]", UserInfo.FirstAndLastName);

            ProjectsEntity projectEntity = new App.ProjectApplication().Get(model.ProjectID);

            string time = string.Empty;
            if (model.AllDay)
                time = model.FromDay.ToString("MM/dd/yyyy");
            else
            {
                model.FromDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
                    , model.FromDay.Month, model.FromDay.Day, model.FromDay.Year, model.FromTime, model.FromTimeType == 1 ? "AM" : "PM"));
                model.ToDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
                    , model.ToDay.Month, model.ToDay.Day, model.ToDay.Year, model.ToTime, model.ToTimeType == 1 ? "AM" : "PM"));

                time = string.Format("{0}  ----  {1}", model.FromDay.ToString("MM/dd/yyyy hh:mm tt"), model.ToDay.ToString("MM/dd/yyyy hh:mm tt"));
            }

            string subject = string.Empty;
            string content = string.Empty;

            foreach (EventInviteEntity item in inviteList)
            {
                if (item.UserID == 0)
                {
                    if (string.IsNullOrEmpty(item.Email) || item.Email.Trim() == string.Empty)
                        continue;
                }
                else
                {
                    UsersEntity user = new App.UserApplication().GetUser(item.UserID);
                    item.FirstName = user.FirstName;
                    item.LastName = user.LastName;
                    item.Email = user.Email;
                }

                string from = Config.DefaultSendEmail;
                if (isDelete)
                {
                    subject = string.Format("[{0}]This event has been canceled. ", model.Name);
                    content = DeleteContent.Replace("[ClientName]", item.FirstName).Replace("[Project]", projectEntity.Title)
           .Replace("[Title]", model.Name).Replace("[Where]", model.Where)
           .Replace("[Detail]", model.Details).Replace("[Time]", time);
                }
                else
                {
                    switch (item.OptionStatus)
                    {
                        case 1: //正常
                            if (isEditDate)
                            {
                                subject = string.Format("[{0}]The time of this event has been changed.", model.Name);
                                content = UpdateContent.Replace("[ClientName]", item.FirstName).Replace("[Project]", projectEntity.Title)
                       .Replace("[Title]", model.Name).Replace("[Where]", model.Where)
                       .Replace("[Detail]", model.Details).Replace("[Time]", time);
                            }
                            else
                                continue;
                            break;
                        case 2: //新加的
                            subject = string.Format("[{0}]You are invited to attend this event.", model.Name);
                            content = AddContent.Replace("[ClientName]", item.FirstName).Replace("[Project]", projectEntity.Title)
                   .Replace("[Title]", model.Name).Replace("[Where]", model.Where)
                   .Replace("[Detail]", model.Details).Replace("[Time]", time);
                            break;
                        case 3: //删除的
                            subject = string.Format("[{0}]You are not going to attend this event.", model.Name);
                            content = RemoveUserContent.Replace("[ClientName]", item.FirstName).Replace("[Project]", projectEntity.Title)
                   .Replace("[Title]", model.Name).Replace("[Where]", model.Where)
                   .Replace("[Detail]", model.Details).Replace("[Time]", time);
                            break;
                    }
                }
                ObjectFactory.GetInstance<IEmailSender>().SendMail(item.Email, Config.DefaultSendEmail, subject, content);

                SendEmail(isEditDate);
            }
        }
        protected void SendEmail(bool isEditDate = false)
        {
            if (isEditDate)
            {
                if (CurrentPTOHoursNotEnough())
                {
                    string content = GetEmailExecuter("PTOBalance .txt").Replace("[Host]", UserInfo.FirstAndLastName);
                    //send one email to user
                    string subject = string.Format("PTO Balance not have enough");
                    content = content.Replace("[ClientName]", UserInfo.FirstName);
                    ObjectFactory.GetInstance<IEmailSender>().SendMail("cs@sunnet.us", Config.DefaultSendEmail, subject, content);
                }
            }
        }
        private bool CurrentPTOHoursNotEnough()
        {
            var StartDate = new DateTime(DateTime.Now.Year, 1, 1);
            var EndDate = StartDate.AddYears(1).AddDays(-1);
            var ptopro = projApp.GetAllProjects().Where(c => c.Title == "0_PTO").FirstOrDefault();
            var CurrentUserpto = PtosHelper.ReGeneratePtos(ptopro.ID, StartDate, EndDate, UserInfo.UserID).FirstOrDefault();
            if (CurrentUserpto != null && CurrentUserpto.Remaining <= 0)
            {
                return true;
            }
            return false;
        }
    }
}