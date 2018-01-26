using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;

namespace SunNet.PMNew.PM2014.Ticket
{
    public partial class ConfirmEstimates : BasePage
    {
        public int TicketID { get; set; }
        TicketsApplication ticketApp = new TicketsApplication();
        FeedBackApplication fbAPP = new FeedBackApplication();
        FileApplication fileApp = new FileApplication();
        TicketsEntity _ticketEntity;
        ProjectApplication projApp = new ProjectApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Pop)this.Master).Width = 450;
            TicketID = QS("tid", 0);
            _ticketEntity = ticketApp.GetTickets(TicketID);

            if (!IsPostBack)
            {
                if (_ticketEntity != null)
                {

                    litHead.Text = "Ticket ID: " + _ticketEntity.TicketID + ", " + _ticketEntity.Title;

                    if (_ticketEntity.Status == TicketsState.Waiting_Confirm)
                    {
                        ltrlFinalTime.Text = _ticketEntity.FinalTime.ToString() + " hour(s)";
                    }

                    ddlClient.DataSource = projApp.GetPojectClientUsers(_ticketEntity.ProjectID, projApp.Get(_ticketEntity.ProjectID).CompanyID);
                    ddlClient.DataBind();

                    ListItem li = ddlClient.Items.FindByValue(_ticketEntity.ConfirmEstmateUserId.ToString());
                    if (li != null) li.Selected = true;
                }
            }
        }



        protected void btnAgree_Click(object sender, EventArgs e)
        {
            _ticketEntity.Status = TicketsState.Estimation_Approved;
            _ticketEntity.ModifiedBy = UserInfo.UserID;
            _ticketEntity.ModifiedOn = DateTime.Now;
            bool result = ticketApp.UpdateTickets(_ticketEntity);

            if (result)
                Redirect(EmptyPopPageUrl, false, true);
            else
                ShowFailMessageToClient(ticketApp.BrokenRuleMessages);
        }

        protected void btnRefusal_Click(object sender, EventArgs e)
        {
            _ticketEntity.Status = TicketsState.Denied;
            _ticketEntity.ModifiedBy = UserInfo.UserID;
            _ticketEntity.ModifiedOn = DateTime.Now;
            ProjectsEntity projectEntity = projApp.Get(_ticketEntity.ProjectID);
            bool result = ticketApp.UpdateTickets(_ticketEntity, true, projectEntity.PMID);

            if (result)
                Redirect(EmptyPopPageUrl, false, true);
            else
                ShowFailMessageToClient(ticketApp.BrokenRuleMessages);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _ticketEntity = ticketApp.GetTickets(TicketID);
            if (_ticketEntity.ConfirmEstmateUserId != int.Parse(ddlClient.SelectedValue))
            {
                ticketApp.UpdateConfirmEstmateUserId(TicketID, int.Parse(ddlClient.SelectedValue));
                new TicketStatusManagerApplication().SendEmailWaitConfirm(_ticketEntity);
            }
            Redirect(EmptyPopPageUrl, false, true);
        }
    }
}