using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class AddFeedBacks : BaseWebsitePage
    {
        #region declare

        FeedBackApplication fbAPP = new FeedBackApplication();
        TicketsApplication ticketsApp = new TicketsApplication();
        GetProjectIdAndUserIDResponse ProjectIDResponse = new GetProjectIdAndUserIDResponse();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            int tid = QS("tid", 0);
            int feedbackId = QS("feedbackId", 0);
            if (tid == 0 && feedbackId == 0)
            {
                this.ShowArgumentErrorMessageToClient();
                return;
            }

            #region role

            FeedBacksEntity entity = new FeedBacksEntity();
            if (feedbackId > 0)
            {
                entity = fbAPP.GetFeedBacksEntity(feedbackId);
                if (!CheckSecurity(entity.TicketID))
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }
                entity.Title = string.Format("<strong>Re:{0}</strong>", entity.Title.Replace("<strong>", "").Replace("</strong>", ""));
                AddFeedBack1.IsReply = true;
                Page.Title = "Reply Feedback";
            }
            else
            {
                if (!CheckSecurity(tid))
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }
                if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.CLIENT)
                {
                    entity.IsPublic = true;
                }
                else
                {
                    entity.IsPublic = false;
                }
                entity.TicketID = tid;
                Page.Title = "Add FeedBacks";
            }

            ProjectIDResponse = ticketsApp.GetProjectIdAndUserID(entity.TicketID);
            if (null != ProjectIDResponse)
            {
                this.AddFeedBack1.ProjectID = ProjectIDResponse.ProjectId;
            }
            this.AddFeedBack1.FeedBacksEntityInfo = entity;

            #endregion
        }

        private bool CheckSecurity(int ticketId)
        {
            if (UserInfo.Role == RolesEnum.CLIENT)
                return false;
            return true;
        }
    }
}
