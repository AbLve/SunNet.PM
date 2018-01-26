using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Ticket.Sunnet
{
    public partial class DevReview : BasePage
    {
        public int TicketID { get; set; }
        readonly TicketsApplication ticketApp = new TicketsApplication();
        protected TicketsEntity _ticketEntity;
        ProjectApplication projApp = new ProjectApplication();

        private bool IsETA
        {
            get
            {
                return _ticketEntity != null
                    && _ticketEntity.IsEstimates
                    && _ticketEntity.Status == TicketsState.Waiting_For_Estimation
                    && _ticketEntity.EsUserID == UserInfo.UserID;
            }
        }

        private List<ListItem> NextStates
        {
            get
            {
                return ConvertStateListToItemList(IsETA ? ticketApp.DevEstimationNext : ticketApp.DevNextStates);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserInfo.Role == Entity.UserModel.RolesEnum.Leader)
            {
                ((Pop)(this.Master)).Width = 770;
            }
            else
                ((Pop)(this.Master)).Width = 570;
            TicketID = QS("tid", 0);
            _ticketEntity = ticketApp.GetTickets(TicketID);
            if (!IsPostBack)
            {
                if (_ticketEntity != null)
                {
                    //绑定Dev可以做的操作状态
                    //绑定是否显示估时项dvEstimationSectoin
                    if (IsETA)
                    {
                        phlEstimation.Visible = true;
                        txtBoxExtimationHours.Text = _ticketEntity.InitialTime == 0
                            ? ""
                            : _ticketEntity.InitialTime.ToString();
                    }

                    if (UserInfo.Role == Entity.UserModel.RolesEnum.Leader)
                    {
                    }
                    else
                    {
                        dvUserView.Style.Add("visibility", "hidden");
                        dvUserView.Style.Add("overflow", "auto");
                        dvUserView.Style.Add("width", "1px");
                        dvUserView.Style.Add("height", "1px");
                    }

                    litHead.Text = "Ticket ID: " + _ticketEntity.TicketID + ", " + _ticketEntity.Title;
                    ddlStatus.Items.AddRange(NextStates.ToArray());
                    ddlResponsibleUser.Items.Add(new ListItem("System", "-1"));
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //保存状态
            //保存估时时间。
            bool result = true;
            if (IsETA)
            {
                decimal etahours = 0;
                if (decimal.TryParse(txtBoxExtimationHours.Text, out etahours))
                {
                    _ticketEntity.InitialTime = etahours;
                }
            }
            if (!result)
            {
                ShowFailMessageToClient(ticketApp.BrokenRuleMessages);
                return;
            }
            var currentState = ddlStatus.SelectedValue.ToEnum<TicketsState>();
            _ticketEntity.Status = currentState;

            int responsibleUserId;
            int.TryParse(QF(ddlResponsibleUser.UniqueID), out responsibleUserId);
            int oldResponsibleUserId = _ticketEntity.ResponsibleUser;
            _ticketEntity.ResponsibleUser = responsibleUserId;

            //如果是Leader还要保存User
            if (UserInfo.Role == Entity.UserModel.RolesEnum.Leader)
            {

                List<TicketUsersEntity> userList = ticketApp.GetTicketUserList(_ticketEntity.TicketID);

                result = ticketApp.AssignUser(_ticketEntity, userViews.GetSelectedUserList()
                    , userList.Where(r => r.Type != TicketUsersType.Dev || r.Type != TicketUsersType.QA).Select(r=>r.UserID).ToList(),UserInfo);
            }
            _ticketEntity.ModifiedBy = UserInfo.UserID;
            _ticketEntity.ModifiedOn = DateTime.Now;
            if (result)
            {
                //sent email to responsible user 2017/10/23
                if (oldResponsibleUserId != _ticketEntity.ResponsibleUser)
                {
                    ticketApp.SendEmailToResponsibile(_ticketEntity, UserInfo);
                }
                if (ticketApp.UpdateTickets(_ticketEntity))
                    Redirect(EmptyPopPageUrl, false, true);
                else
                    ShowFailMessageToClient(ticketApp.BrokenRuleMessages);
            }
            else
            {
                ShowFailMessageToClient(ticketApp.BrokenRuleMessages);
            }

        }
    }
}