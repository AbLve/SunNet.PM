using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class AddTask : BaseWebsitePage
    {
        TicketsApplication ticketApp = new TicketsApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            int taskid = QS("taskid", 0);
            int ticketid = QS("tid", 0);

            if (!IsPostBack)
            {
                if (taskid == 0 && ticketid == 0)
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }
                if (taskid > 0)
                {
                    Page.Title = "View Task Detail";
                    TasksEntity ta = ticketApp.GetTaskEntity(taskid);
                    if (CheckSecurity(ta, 0))
                    {
                        BindData(ta);
                        EnableControl(ta.IsCompleted);
                    }
                }
                else
                {
                    Page.Title = "Add New Task";
                    if (CheckSecurity(null, ticketid))
                    {
                        this.btnClear.Visible = true;
                        this.btnSave.Visible = true;
                    }
                }
            }

        }

        private bool CheckSecurity(TasksEntity ta, int tid)
        {
            GetTicketCreateByAndStatusResponse response = new GetTicketCreateByAndStatusResponse();
            if (ta == null)
            {
                response = ticketApp.GetTicketCreateByAndStatus(tid);
            }
            else
            {
                response = ticketApp.GetTicketCreateByAndStatus(ta.TicketID);
            }

            if ((response.CreateUserId == UserInfo.UserID || allowAddTaskUser.Contains(UserInfo.RoleID)) &&
                response.status >= (int)TicketsState.Submitted &&
                response.status != (int)TicketsState.Estimation_Fail &&
                response.status != (int)TicketsState.Completed &&
                response.status != (int)TicketsState.Cancelled)
            {
                return true;
            }
            return false;
        }
        //allow add user
        int[] allowAddTaskUser = { (int)RolesEnum.ADMIN, (int)RolesEnum.PM,
                                   (int)RolesEnum.DEV,(int)RolesEnum.QA,
                                   (int)RolesEnum.Leader,(int)RolesEnum.Contactor};

        private void BindData(TasksEntity ta)
        {
            this.txtTitle.Value = ta.Title;
            this.txtDesc.Value = ta.Description;

            if (ta.IsCompleted)
            {
                this.IsComplete.Visible = true;
                this.Complete.Visible = true;
                this.IsCompleteCK.Checked = true;
            }
            this.txtComplete.Text = ta.CompletedDate.ToString("MM/dd/yyyy");
        }

        private void EnableControl(bool status)
        {
            this.IsCompleteCK.Attributes["disabled"] = "disabled";
            this.txtComplete.Enabled = false;
            this.txtTitle.Attributes["readonly"] = "readonly";
            this.txtDesc.Attributes["readonly"] = "readonly";
        }


    }
}
