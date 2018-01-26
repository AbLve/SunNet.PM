using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.ProjectModel;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class AssignUsers : BaseWebsitePage
    {
        #region declare

        TicketsApplication ticketApp = new TicketsApplication();
        UserApplication userApp = new UserApplication();
        ProjectApplication projectApp = new ProjectApplication();
        List<ProjectUsersEntity> projectUsersList = new List<ProjectUsersEntity>();
        List<UsersEntity> userList = new List<UsersEntity>();
        GetProjectIdAndUserIDResponse response;
        List<TicketUsersEntity> tiketUserEntityList = new List<TicketUsersEntity>();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (QS("tid", 0) <= 0)
            {
                this.ShowArgumentErrorMessageToClient();
                return;
            }
            response = ticketApp.GetProjectIdAndUserID(QS("tid", 0));
            projectUsersList = projectApp.GetProjectSunnetUserList(response.ProjectId);
            foreach (ProjectUsersEntity tmpU in projectUsersList)
            {
                UsersEntity user = userApp.GetUser(tmpU.UserID);
                if (user != null)
                {
                    if (user.Status.Trim() != "INACTIVE")
                    {
                        userList.Add(user);
                    }
                }
            }
            if (userList.Count <= 0)
            {
                this.ShowMessageToClient("No user under this project.", 0, false, false);
            }
            tiketUserEntityList = ticketApp.GetListUsersByTicketId(QS("tid", 0));
        }


        public string ShowIsExitsInTicketUser(int uid)
        {
            string check = "";
            TicketUsersType tmpType = (TicketUsersType)int.Parse(this.ddlRole.SelectedValue);
            TicketUsersEntity tmpTicketUsersEntity = tiketUserEntityList.Find(r => r.UserID == uid && r.Type == tmpType);
            if (null != tmpTicketUsersEntity)
            {
                check = string.Format("<input type='checkbox' checked='checked'id='{0}' />", uid);
            }
            else
            {
                check = string.Format("<input type='checkbox' id='{0}' />", uid);
            }
            return check;
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlRole.SelectedIndex <= 0) return;
            TicketUsersType tmpType = (TicketUsersType)int.Parse(this.ddlRole.SelectedValue);
            if (tmpType == TicketUsersType.Dev)
            {
                this.rptAssignUser.DataSource = userList.FindAll(r => (r.Role == RolesEnum.DEV
                    || r.Role == RolesEnum.Leader || r.Role == RolesEnum.Contactor));
            }
            else if (tmpType == TicketUsersType.QA)
                this.rptAssignUser.DataSource = userList.FindAll(r => r.Role == RolesEnum.QA);
            else if (tmpType == TicketUsersType.Other)
                this.rptAssignUser.DataSource = from a in userList
                                                where (from b in tiketUserEntityList
                                                       where a.UserID.Equals(b.UserID)
                                                       select b.UserID).Count() == 0
                                                select a;
            this.rptAssignUser.DataBind();
        }
    }
}
