using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.PM2014.Ticket
{
    public partial class Approve : BasePage
    {
        TicketsApplication ticketAPP = new TicketsApplication();
        TicketStatusManagerApplication ticketStatusMgr = new TicketStatusManagerApplication();
        FeedBackApplication fbAPP = new FeedBackApplication();
        TicketsEntity _ticketEntity;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ((Pop)this.Master).Width = 500;
                int ticketID = QS("tid", 0);
                _ticketEntity = ticketAPP.GetTickets(ticketID);
                litHead.Text = "Ticket ID: " + _ticketEntity.TicketID + ", " + _ticketEntity.Title;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //添加ticket的Rating, 和Feedback
            int star;
            if (!int.TryParse(hdStar.Value, out star))
            {
                star = 0;
            }
            int ticketID = QS("tid", 0);
            if (ticketID != 0)
            {
                TicketsApplication ticketsApplication = new TicketsApplication();
                if (ticketsApplication.UpdateTicketStar(ticketID, star))
                {
                    //添加Feedback
                    bool isSuccess = UpdateTicketStatusToApprove(ticketID);

                    if (isSuccess)
                    {
                        if (txtFeedback.Text.Trim() != string.Empty)
                        {
                            FeedBacksEntity feedbacksEntity = new FeedBacksEntity();
                            feedbacksEntity.IsDelete = false;
                            feedbacksEntity.TicketID = ticketID;
                            feedbacksEntity.Title = "";
                            feedbacksEntity.Description = txtFeedback.Text.NoHTML();
                            feedbacksEntity.CreatedBy = UserInfo.UserID;
                            feedbacksEntity.CreatedOn = DateTime.Now;
                            feedbacksEntity.ModifiedOn = DateTime.Now;
                            feedbacksEntity.IsPublic = true;
                            feedbacksEntity.WaitClientFeedback = FeedbackReplyStatus.Normal;
                            feedbacksEntity.WaitPMFeedback = FeedbackReplyStatus.Normal;

                            feedbacksEntity.ID = fbAPP.AddFeedBacks(feedbacksEntity);

                            if (feedbacksEntity.ID <= 0)
                            {
                                ShowFailMessageToClient();
                                return;
                            }
                            ticketStatusMgr.SendEmailtoPMForFeedBack(feedbacksEntity);//状态更新，不进行刷新气泡
                            //if (ticketAPP.CreateNotification(ticketID,UserInfo.UserID))
                            //{
                            //    //发邮件给PM
                            //    ticketStatusMgr.SendEmailtoPMForFeedBack(feedbacksEntity);
                            //}
                        }

                        string returnurl = Request.QueryString["returnurl"];
                        if (string.IsNullOrEmpty(returnurl))
                        {
                            Redirect(Request.RawUrl, false, true);
                        }
                        else
                        {
                            ParentToUrl(returnurl);
                        }
                    }
                    else
                    {
                        ShowFailMessageToClient("Update ticket’s status fail.");
                    }
                }
            }
            else
            {
                ShowFailMessageToClient("Update ticket’s status fail.");
            }
        }

        public bool UpdateTicketStatusToApprove(int ticketID)
        {
            try
            {
                if (UserInfo.UserID <= 0)
                    return false;
                TicketsEntity ticketEntity = ticketAPP.GetTickets(ticketID);
                TicketsState originalStatus = ticketEntity.Status;
                bool Update = true;
                ticketEntity.Status = TicketsState.Completed;
                ticketEntity.ModifiedOn = DateTime.Now;
                ticketEntity.ModifiedBy = UserInfo.UserID;
                ticketEntity.PublishDate = DateTime.Now.Date;
                Update = ticketAPP.UpdateTickets(ticketEntity);

                // 完成时更新所有相关人员的WorkingOn状态
                ticketAPP.UpdateWorkingOnStatus(ticketID, TicketsState.Completed);
                return Update;
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(string.Format("Error Ashx:DoUpdateTicketStatus.ashx Messages:\r\n{0}", ex));
                return false;
            }

        }
    }
}