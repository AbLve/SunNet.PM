using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class TicketUsers : BaseAscx
    {
        TicketsApplication ticketApp = new TicketsApplication();
        ProjectApplication projApp = new ProjectApplication();
        UserApplication userApp = new UserApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            int tid = QS("tid", 0);
            if (tid > 0)
            {
                BindUserInfo(tid);
            }
        }


        private void BindUserInfo(int id)
        {
            List<TicketUsersEntity> list = ticketApp.GetListUsersByTicketId(id);
            if (list != null && list.Count > 0)
            {
                foreach (TicketUsersEntity u in list)
                {
                    if (u.Type == TicketUsersType.PM)
                    {
                        this.lilPmName.Text += BaseWebsitePage.GetClientUserName(u.UserID) + " ; ";
                    }
                    else if (u.Type == TicketUsersType.Dev)
                        this.lilDevName.Text += BaseWebsitePage.GetClientUserName(u.UserID) + " ; ";
                    else if (u.Type == TicketUsersType.QA)
                        this.lilTestName.Text += BaseWebsitePage.GetClientUserName(u.UserID) + " ; ";
                    else if (u.Type == TicketUsersType.Other)
                        this.lilOtherName.Text += BaseWebsitePage.GetClientUserName(u.UserID) + " ; ";
                }
            }
        }
    }
}