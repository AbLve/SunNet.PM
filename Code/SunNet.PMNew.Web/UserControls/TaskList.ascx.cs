using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.UserModel;
namespace SunNet.PMNew.Web.UserControls
{
    public partial class TaskList : BaseAscx
    {
        TicketsApplication ticketApp = new TicketsApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            int tid = QS("tid", 0);
            if (tid > 0)
            {
                GetTaskListByTid(tid);
            }
        }

        private void GetTaskListByTid(int tid)
        {
            List<TasksEntity> list = new List<TasksEntity>();
            list = ticketApp.GetTaskByID(tid, true);
            if (null == list || list.Count <= 0)
            {
                this.trNoTickets.Visible = true;
            }
            this.rptTaskList.DataSource = list;
            this.rptTaskList.DataBind();
        }

        public string ShowUpdateTaskStatus(int taskId, int ticketId, bool isComp)
        {
            GetTicketCreateByAndStatusResponse response = ticketApp.GetTicketCreateByAndStatus(ticketId);
            int createId = response.CreateUserId;

            string task = "";

            if ((createId == UserInfo.UserID || UserInfo.Role == RolesEnum.PM ||
                UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.Leader ||
                UserInfo.Role == RolesEnum.DEV || UserInfo.Role == RolesEnum.QA) ||
                UserInfo.Role == RolesEnum.Contactor &&
                isComp != true)
            {
                task = string.Format(@"<a href='#' onclick='updateTaskStatus({0});return false;'  >
                            <img src='/icons/20.gif' border='0' title='Complete' alt='Complete'></a>", taskId);

            }
            return task;
        }
    }
}