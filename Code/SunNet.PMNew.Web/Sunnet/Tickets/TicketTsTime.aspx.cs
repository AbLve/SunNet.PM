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
    public partial class TicketTsTime : BaseWebsitePage
    {
        #region declare
        TicketsApplication ticketApp = new TicketsApplication();
        UserApplication userApp = new UserApplication();
        int count = 0;
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            int tid = QS("tid", 0);
            DateBind(tid);
            this.HdIsPm.Value = UserInfo.Role == RolesEnum.PM ? "true" : "false";
        }

        private void DateBind(int tid)
        {
            List<TicketEsTime> list = ticketApp.GetListEs(tid);

            List<TicketEsTime> listPm = list.FindAll(x => x.IsPM == true);

            if (list.Count > 0 && listPm.Count <= 0 && UserInfo.Role == RolesEnum.PM)
            {
                List<TicketEsTime> listCopy = list.FindAll(x => x.IsPM == false);
                foreach (TicketEsTime item in listCopy)
                {
                    item.CreatedTime = DateTime.Now.Date;
                    item.EsByUserId = UserInfo.ID;
                    item.IsPM = true;
                    ticketApp.AddEsTime(item);
                }
                decimal pmTime = listCopy.Count > 0 ? listCopy.Sum(x => x.TotalTimes) : 0;
                ticketApp.UpdateEs(pmTime, tid, UserInfo.ID, true);
            }
        }
    }
}
