using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.PM2014.SunnetTicket
{
    public partial class SalerReview : BasePage
    {
        public int TicketID { get; set; }
        TicketsApplication ticketApp = new TicketsApplication();
        FeedBackApplication fbAPP = new FeedBackApplication();
        FileApplication fileApp = new FileApplication();
        TicketsEntity _ticketEntity; 
        ProjectApplication projApp = new ProjectApplication();
        UserApplication userApp = new UserApplication();

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
                        ltrlFinalTime.Text = _ticketEntity.FinalTime.ToString() + " h";

                        List<TicketUsersEntity> userList = ticketApp.GetTicketUserList(_ticketEntity.TicketID);

                        foreach (TicketUsersEntity ticketUser in userList)
                        {
                            UsersEntity user = userApp.GetUser(ticketUser.UserID);
                            ddlResponsibleUser.Items.Add(new ListItem() { Value = user.UserID.ToString(), Text = user.FirstAndLastName });
                        }
                        ddlResponsibleUser.Items.Add(new ListItem("System", "-1"));
                        ListItem li = ddlResponsibleUser.Items.FindByValue(_ticketEntity.ResponsibleUser.ToString());
                        if (li != null)
                            li.Selected = true;

                    }
                    else
                        trResponsible.Visible = true;
                }
            }
        }

           

        protected void btnAgree_Click(object sender, EventArgs e)
        {
            int oldResponsibleUserId = _ticketEntity.ResponsibleUser;
            _ticketEntity.Status = TicketsState.Estimation_Approved;
            _ticketEntity.ModifiedBy = UserInfo.UserID;
            _ticketEntity.ModifiedOn = DateTime.Now;
            _ticketEntity.ResponsibleUser = int.Parse(ddlResponsibleUser.SelectedValue);

            ProjectsEntity projectEntity = projApp.Get(_ticketEntity.ProjectID);

              bool result = ticketApp.UpdateTickets(_ticketEntity, true, projectEntity.PMID);

            //sent email to responsible user 2017/10/23
            if (oldResponsibleUserId != _ticketEntity.ResponsibleUser)
            {
                ticketApp.SendEmailToResponsibile(_ticketEntity, UserInfo);
            }
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
            _ticketEntity.ResponsibleUser = int.Parse(ddlResponsibleUser.SelectedValue);
            bool result = ticketApp.UpdateTickets(_ticketEntity,true,_ticketEntity.ResponsibleUser);
           
            if (result)
                Redirect(EmptyPopPageUrl, false, true);
            else
                ShowFailMessageToClient(ticketApp.BrokenRuleMessages);
        }

    }
}