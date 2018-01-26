using SunNet.PMNew.App;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using System.IO;
using System.Configuration;
using SunNet.PMNew.PM2014.OA.Pto;

namespace SunNet.PMNew.PM2014.Event
{
    public partial class Add : BasePage
    {
        private ProjectApplication projApp;
        private EventsApplication eventApp;

        public string HRProjectID = Config.HRProjectID;
        public bool NotEnoughPTOHour = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            projApp = new ProjectApplication();
            eventApp = new EventsApplication();
            if (!Page.IsPostBack)
            {
                NotEnoughPTOHour = CurrentPTOHoursNotEnough();
                DateTime eventDate = QS("Date", DateTime.Now);
                txtFrom.Text = eventDate.ToString("MM/dd/yyyy");
                txtTo.Text = eventDate.ToString("MM/dd/yyyy");
                txtEndDate.Text = eventDate.ToString("MM/dd/yyyy");
                List<ProjectDetailDTO> projectList = projApp.GetUserProjectsForCreateObject(UserInfo);
                EventEntity eventNewest = eventApp.GetEventByCreateId(UserInfo.UserID);
                switch (UserInfo.Role)
                {
                    case RolesEnum.Leader:
                    case RolesEnum.DEV:
                    case RolesEnum.QA:
                        projectList = projectList.FindAll(r => r.CompanyID == Config.SunnetCompany);
                        break;
                }
                if (eventNewest != null)
                {
                    var projectNewest = projectList.FirstOrDefault(c => c.ProjectID == eventNewest.ProjectID);
                    if (projectNewest != null)
                    {
                        projectList.Remove(projectNewest);
                        projectList.Insert(0, projectNewest);
                    }
                    projectList.BindDropdown<ProjectDetailDTO>(ddlProjects, "Title", "ProjectID");
                    ddlProjects.Items.FindByValue(eventNewest.ProjectID.ToString()).Selected = true;
                }
                else
                {
                    projectList.BindDropdown<ProjectDetailDTO>(ddlProjects, "Title", "ProjectID", "Please select...", "");
                }
                userName.Text = UserInfo.FirstName;
                int searchedProjectID = QS("pid", -1);
                ddlProjects.SelectedValue = searchedProjectID.ToString();
                ((Pop)(this.Master)).Width = 780;

            }
        }

        private EventsView ConstructEventsView()
        {
            EventsView eventsView = new EventsView();
            eventsView.Alert = (AlertType)int.Parse(ddlAlert.SelectedItem.Value);
            eventsView.AllDay = chkAllDay.Checked;
            eventsView.CreatedBy = UserInfo.UserID;
            eventsView.CreatedOn = DateTime.Now;
            eventsView.Details = txtDetails.Text.Trim().NoHTML();
            eventsView.End = (EndType)int.Parse(ddlEnd.SelectedValue);
            eventsView.EndDate = DateTime.Parse(txtEndDate.Text);

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

            eventsView.Name = txtName.Text.Trim().NoHTML();
            eventsView.ProjectID = int.Parse(ddlProjects.SelectedValue);
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
            eventsView.Repeat = (RepeatType)int.Parse(selectRepeat.SelectedValue);
            eventsView.RoleIDs = ((int)Privates.OnlyMe).ToString();
            eventsView.Times = int.Parse(txtTimes.Text);
            eventsView.UserIds = "";
            eventsView.Where = txtWhere.Text.Trim().NoHTML();
            eventsView.Icon = string.IsNullOrEmpty(Icon.Value) ? 8 : int.Parse(Icon.Value);
            eventsView.IsOff = chkOff.Checked;
            return eventsView;
        }

        private List<EventInviteEntity> BuilderInvite(EventsView model)
        {
            List<EventInviteEntity> inviteList = new List<EventInviteEntity>();
            string projectIds = QF("chkProjectUser");
            if (projectIds.Trim() != string.Empty)
            {
                List<int> listUserId = new App.ProjectApplication().GetActiveUserIdByProjectId(model.ProjectID);
                projectIds = projectIds.Trim();
                if (projectIds.EndsWith(","))
                    projectIds = projectIds.Remove(projectIds.Length - 1);
                int tmpId;
                foreach (string s in projectIds.Split(','))
                {
                    if (int.TryParse(s, out tmpId))
                    {
                        if (listUserId.Contains(tmpId))
                        {
                            inviteList.Add(new EventInviteEntity()
                            {
                                CreatedID = UserInfo.UserID,
                                EventID = 0,
                                FromDay = DateTime.Now,
                                UserID = tmpId,
                                Email = "",
                                FirstName = "",
                                LastName = ""
                            });
                        }
                    }
                }
            }
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
                    email = QF("txtOtherUserEmail" + i).Trim();
                    if (firstName != string.Empty && lastName != string.Empty)
                    {
                        inviteList.Add(new EventInviteEntity()
                        {
                            CreatedID = UserInfo.UserID,
                            LastName = lastName,
                            FirstName = firstName,
                            Email = email,
                            FromDay = DateTime.Now,
                            EventID = 0,
                            UserID = 0
                        });
                    }
                }
            }
            return inviteList;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlProjects.SelectedValue == "")
            {
                ShowFailMessageToClient("Please select project.");
                return;
            }
            if (txtName.Text.Trim() == string.Empty)
            {
                ShowFailMessageToClient("Please enter title.");
                return;
            }
            EventsView eventModel = ConstructEventsView();
            List<EventInviteEntity> inviteList = BuilderInvite(eventModel);
            List<EventEntity> eventList;

            //当开始日期和结束日期不是同一天且Repeat为None时，需要处理成Repeat为Every Day、且End为On date,EndDate为选择的结束日期
            if (eventModel.ToDay.Date > eventModel.FromDay.Date && eventModel.Repeat == RepeatType.None)
            {
                eventModel.Repeat = RepeatType.Every_Day;
                eventModel.End = EndType.on_date;
                eventModel.EndDate = eventModel.ToDay;
            }
            if (new EventsApplication().AddEvents(eventModel, inviteList, out eventList))
            {
                SendHandler handler = new SendHandler(SendEamil);
                handler.BeginInvoke(inviteList, eventList, null, null);
                SendEmail();
                Redirect(Request.RawUrl, false, true);
            }
            else
            {
                ShowFailMessageToClient("Add event Fail.");
            }
        }

        public delegate void SendHandler(List<EventInviteEntity> inviteList, List<EventEntity> eventList);

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

        protected void SendEamil(List<EventInviteEntity> inviteList, List<EventEntity> eventList)
        {
            string addcontent = GetEmailExecuter("CreateEvent.txt").Replace("[Host]", UserInfo.FirstAndLastName);

            foreach (EventEntity eventEntity in eventList)
            {
                ProjectsEntity projectEntity = new App.ProjectApplication().Get(eventEntity.ProjectID);
                string time = string.Empty;
                if (eventEntity.AllDay)
                    time = eventEntity.FromDay.ToString("MM/dd/yyyy");
                else
                {
                    eventEntity.FromDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
                        , eventEntity.FromDay.Month, eventEntity.FromDay.Day, eventEntity.FromDay.Year, eventEntity.FromTime, eventEntity.FromTimeType == 1 ? "AM" : "PM"));
                    eventEntity.ToDay = DateTime.Parse(string.Format("{0}/{1}/{2} {3} {4}"
                        , eventEntity.ToDay.Month, eventEntity.ToDay.Day, eventEntity.ToDay.Year, eventEntity.ToTime, eventEntity.ToTimeType == 1 ? "AM" : "PM"));

                    time = string.Format("{0}  ----  {1}", eventEntity.FromDay.ToString("MM/dd/yyyy hh:mm tt"), eventEntity.ToDay.ToString("MM/dd/yyyy hh:mm tt"));
                }

                string subject = string.Format("[{0}]You are invited to attend this event.", eventEntity.Name);

                foreach (EventInviteEntity item in inviteList)
                {
                    string from = Config.DefaultSendEmail;
                    if (item.UserID > 0)
                    {
                        UsersEntity user = new App.UserApplication().GetUser(item.UserID);
                        item.FirstName = user.FirstName;
                        item.LastName = user.LastName;
                        item.Email = user.Email;
                    }
                    if (item.Email.Trim() != string.Empty)
                    {
                        string content = addcontent.Replace("[ClientName]", item.FirstName).Replace("[Project]", projectEntity.Title)
                        .Replace("[Title]", eventEntity.Name).Replace("[Where]", eventEntity.Where)
                        .Replace("[Detail]", eventEntity.Details).Replace("[Time]", time);
                        ObjectFactory.GetInstance<IEmailSender>().SendMail(item.Email, Config.DefaultSendEmail, subject, content);
                    }
                }
            }
        }

        protected void SendEmail()
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