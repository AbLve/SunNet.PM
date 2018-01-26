using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.CompanyModel;
using System.IO;
using SunNet.PMNew.Entity.ProjectModel;
using System.Text;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class TicketDetail : BaseWebsitePage
    {
        TicketsApplication ticketApp = new TicketsApplication();
        bool hasPermission = true;
        ProjectApplication projectApp = new ProjectApplication();
        UserApplication userApp = new UserApplication();
        TicketsEntity ticketEntity = null;
        bool isShowFeedback = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region initial

            this.AddTicket1.IsSunnet = "true";
            this.AddTicket1.IsEnable = true;
            this.FeedBacksList1.IsSunnet = true;
            #endregion
            if (!IsPostBack)
            {
                int tid = QS("tid", 0);
                if (tid <= 0)
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }
                else
                {
                    ticketEntity = ticketApp.GetTickets(tid);
                    if (ticketEntity != null)
                    {
                        hasPermission = ticketApp.ValidateIsExistUserUnderProject(ticketEntity.TicketID, UserInfo.UserID);
                        if (!CheckSecurity(ticketEntity))
                        {
                            this.ShowArgumentErrorMessageToClient();
                            return;
                        }
                        this.lilTicketTitle.Text = " : " + ticketEntity.Title;
                        RelationTicketsList1.RelationhasPermission = hasPermission;
                        AddTicket1.TicketsEntityInfo = ticketEntity;
                        WorkFlow1.WorkFlowTicketEntity = ticketEntity;
                        TicketBaseInfo1.TicketsEntityInfo = ticketEntity;
                        FeedBacksList1.TicketsEntityInfo = ticketEntity;
                    }
                    else
                    {
                        this.ShowArgumentErrorMessageToClient();
                        return;
                    }

                    EnableControl(ticketEntity);
                }
            }
            InitCompanyInfo();
            InitSunnetUsers();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            //对需要加载页上的所有其他控件的任务使用该事件。
            FeedBacksList1.Visible = isShowFeedback;
            base.OnLoadComplete(e);
        }

        private void InitCompanyInfo()
        {
            CompanyApplication comApp = new CompanyApplication();
            CompanysEntity company = comApp.GetCompany(UserInfo.CompanyID);
            if (UserInfo.CompanyID == 1)  //Sunnet 公司
            {
                ltLogo.Text = string.Format("<a href=\"http://www.sunnet.us\" target=\"_blank\">{0}</a>"
                    , BuilderLogo(company.Logo));
            }
            else
            {
                CompanysEntity sunntCompanyEntity = comApp.GetCompany(1);//获取Sunnet公司

                if (company.Logo.IndexOf("logomain.jpg") >= 0) //没有上传Logo ，则显示 Sunnet 公司Logo
                {
                    ltLogo.Text = string.Format("<a href=\"http://www.sunnet.us\" target=\"_blank\">{0}</a>"
                   , BuilderLogo(sunntCompanyEntity.Logo));
                }
                else
                {
                    ltLogo.Text = BuilderLogo(company.Logo);

                    ltSunnetLogo.Text = string.Format("<a href=\"http://www.sunnet.us\" target=\"_blank\">{0}</a>"
          , BuilderLogo(sunntCompanyEntity.Logo));
                }
            }
        }

        private string BuilderLogo(string image)
        {
            string filename = Server.MapPath(image);
            if (File.Exists(filename))
            {
                return string.Format("<img style=\"height:39px;width:126px;border-width:0px;\" src=\"{0}\"/>", image);
            }
            else return string.Format("<img style=\"height:39px;width:126px;border-width:0px;\" src=\"{0}\"/>", "/images/logomain.jpg");
        }

        private void EnableControl(TicketsEntity ticketEntity)
        {
            bool isTicketUser = ticketApp.IsTicketUser(ticketEntity.ID, UserInfo.ID);
            if (!isTicketUser)
            {
                this.pr.Visible = false;
                this.line1.Visible = false;
                this.line2.Visible = false;
                this.line3.Visible = false;
                this.line4.Visible = false;
                this.topMenu.Visible = false;
                this.ng.Visible = false;
                this.divtopMeunFill.Visible = false;
                this.cr.Visible = false;
                divAddRelation.Visible = false;
                divAssign.Visible = false;
                divTask.Visible = false;
            }
            UserApplication userApplication = new UserApplication();
            UsersEntity usersEntity = userApplication.GetUser(ticketEntity.CreatedBy);
            if (((ticketEntity.IsInternal != true &&
                ticketEntity.TicketType != TicketsType.Request) ||
                (usersEntity.Role == RolesEnum.Supervisor)) &&
                ticketEntity.Status != TicketsState.Cancelled &&
                ticketEntity.Status < TicketsState.Developing &&
                ticketEntity.Status != TicketsState.Estimation_Fail &&
                UserInfo.Role != RolesEnum.Sales)
            {
                this.topMenu.Visible = true;

                if (UserInfo.Role != RolesEnum.PM && UserInfo.Role != RolesEnum.ADMIN)
                {
                    this.pr.Visible = false;
                    this.cr.Visible = false;
                    this.line2.Visible = false;
                    this.line1.Visible = false;
                }

                if (ticketEntity.ConvertDelete == CovertDeleteState.NotABug &&
                    (UserInfo.Role != RolesEnum.PM &&
                    UserInfo.Role != RolesEnum.ADMIN))
                {
                    this.topMenu.Visible = false;
                }
                else if ((ticketEntity.ConvertDelete == CovertDeleteState.NotABug ||
                    ticketEntity.ConvertDelete == CovertDeleteState.ConvertToHistory ||
                    ticketEntity.TicketType != TicketsType.Bug) &&
                    (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN))
                {
                    this.line3.Visible = false;
                    this.line4.Visible = false;
                    this.cr.Visible = false;
                    this.ng.Visible = false;
                }
            }

            if ((int)ticketEntity.Status < (int)TicketsState.Submitted ||
                ticketEntity.Status == TicketsState.Completed || !hasPermission)
            {
                if (UserInfo.Role != RolesEnum.ADMIN)
                {
                    isShowFeedback = false;
                }
                divAddRelation.Visible = false;
                divTask.Visible = false;
            }
            else if (UserInfo.Role != RolesEnum.PM &&
                     UserInfo.Role != RolesEnum.Leader &&
                     UserInfo.Role != RolesEnum.DEV &&
                     UserInfo.Role != RolesEnum.Contactor &&
                     UserInfo.Role != RolesEnum.Supervisor &&
                     !hasPermission)
            {
                divTask.Visible = false;
            }

            if (!((UserInfo.Role == RolesEnum.PM ||
                   UserInfo.Role == RolesEnum.ADMIN ||
                   UserInfo.Role == RolesEnum.Leader ||
                   UserInfo.Role == RolesEnum.Sales)
                   && ticketEntity.Status != TicketsState.Cancelled
                   && ticketEntity.Status != TicketsState.Completed) || !hasPermission)
            {
                divAssign.Visible = false;
            }

            if ((UserInfo.UserID != ticketEntity.CreatedBy)
                && (UserInfo.Role != RolesEnum.PM
                && UserInfo.Role != RolesEnum.ADMIN)
                && !hasPermission)
            {
                divAddRelation.Visible = false;
            }
            if (this.topMenu.Visible == false)
            {
                this.divtopMeunFill.Visible = true;
            }

            if (ticketEntity.Status > TicketsState.PM_Reviewed)
            {
                this.line3.Visible = false;
                this.line4.Visible = false;
                this.line2.Visible = false;
                this.pr.Visible = false;
            }

            if (ticketEntity.ConvertDelete == CovertDeleteState.ConvertToHistory)
            {
                this.cr.Visible = false;
                this.line1.Visible = false;
            }

            if (UserInfo.Role == RolesEnum.Supervisor && UserInfo.ID != ticketEntity.CreatedBy)
            {
                topMenu.Visible = false;
                isShowFeedback = false;
                divAddRelation.Visible = false;
                divTask.Visible = false;
            }
        }

        private bool CheckSecurity(TicketsEntity info)
        {
            if (UserInfo.Role == RolesEnum.CLIENT)
                return false;
            return true;
        }

        protected string GetUsersHtml(RolesEnum role)
        {
            StringBuilder htmls = new StringBuilder();
            GetProjectIdAndUserIDResponse response = ticketApp.GetProjectIdAndUserID(QS("tid", 0));
            List<ProjectUsersEntity> projectUsers = projectApp.GetProjectSunnetUserList(response.ProjectId);
            ProjectsEntity projectsEntity = projectApp.Get(response.ProjectId);
            if (projectUsers != null && projectUsers.Count > 0)
            {
                bool isHasUser = false;
                foreach (ProjectUsersEntity projectUser in projectUsers)
                {
                    UsersEntity user = userApp.GetUser(projectUser.UserID);
                    if (user != null && user.Status.Trim() != "INACTIVE")
                    {
                        if (user.Role != RolesEnum.CLIENT && user.Role == role)
                        {
                            if (ticketEntity.CreatedBy != user.UserID)
                            {
                                htmls.AppendFormat("<li userid='{0}' data-role=" + (int)MapRoleToTicketUserType(role) + ">{1}</li>", user.ID, user.FirstName);
                            }
                            else
                            {
                                htmls.AppendFormat("<li userid='{0}' data-isCreate='true' data-role=" + (int)MapRoleToTicketUserType(role) + ">{1}</li>", user.ID, user.FirstName);
                            }
                            isHasUser = true;
                        }
                    }
                }
                if (!isHasUser)
                {
                    htmls.AppendFormat("<li style='{0}'></li>", "visibility:hidden;");
                }
            }
            return htmls.ToString();
        }

        private TicketUsersType MapRoleToTicketUserType(RolesEnum role)
        {
            switch (role)
            {
                case RolesEnum.PM:
                    return TicketUsersType.PM;
                case RolesEnum.DEV:
                    return TicketUsersType.Dev;
                case RolesEnum.QA:
                    return TicketUsersType.QA;
                default: return TicketUsersType.Other;
            }
        }

        private void InitSunnetUsers()
        {
            List<TicketUsersEntity> ticketUsers = ticketApp.GetListUsersByTicketId(QS("tid", 0));
            List<UsersEntity> listCurrentTicketUser = new List<UsersEntity>();

            if (ticketUsers != null && ticketUsers.Count > 0)
            {
                foreach (TicketUsersEntity projectUser in ticketUsers)
                {
                    UsersEntity user = userApp.GetUser(projectUser.UserID);
                    if (user != null && user.Status.Trim() != "INACTIVE" && user.Role != RolesEnum.CLIENT)
                    {
                        listCurrentTicketUser.Add(user);
                    }
                }
            }

            if (listCurrentTicketUser.Count < 1)
            {
                hidSelectedSunneters.Value = "[]";
            }
            else
                hidSelectedSunneters.Value = GetUsersJson(listCurrentTicketUser);
        }

        private string GetUsersJson(List<UsersEntity> listCurrentProject)
        {
            StringBuilder strSelected = new StringBuilder();
            strSelected.Append("[{\"id\":0}");
            foreach (UsersEntity item in listCurrentProject)
            {
                strSelected.Append(",{\"id\":");
                strSelected.Append(item.ID);
                strSelected.Append("}");
            }
            strSelected.Append("]");
            return strSelected.ToString();
        }

    }
}
