using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TicketModel.Enums;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Web.Do;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// TicketUser 的摘要说明
    /// </summary>
    public class TicketUser : DoBase, IHttpHandler
    {
        private TicketsApplication ticketApp = new TicketsApplication();
        private TicketStatusManagerApplication ticketStatusApp = new TicketStatusManagerApplication();
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            context.Response.ContentType = "application/json";
            if (UserID <= 0)
            {
                Response.Write("[]");
                return;
            }
            string action = context.Request.Params["action"].ToLower();
            var ticket = 0;
            var user = UserID;
            var status = TicketUserStatus.None;

            var targetUser = 0;
            var update = 0;
            bool flag = true;
            switch (action)
            {
                case "setworkingon":
                    int.TryParse(Request.Params["ticket"], out ticket);
                    status = TicketUserStatus.WorkingOn;
                    if (ticket > 0 && ticketApp.UpdateWorkingOnStatus(ticket, user, status))
                        Response.Write(ResponseMessage.GetResponse(true));
                    else
                        Response.Write(ResponseMessage.GetResponse(false, ticketApp.FirstBrokenRuleMessageContent));
                    break;
                case "setworkingcomplete":
                    int.TryParse(Request.Params["ticket"], out ticket);
                    status = TicketUserStatus.Completed;
                    if (ticket > 0 && ticketApp.UpdateWorkingOnStatus(ticket, user, status))
                        Response.Write(ResponseMessage.GetResponse(true));
                    else
                        Response.Write(ResponseMessage.GetResponse(false, ticketApp.FirstBrokenRuleMessageContent));
                    break;
                case "setworkingcancelled":
                    int.TryParse(Request.Params["ticket"], out ticket);
                    status = TicketUserStatus.Canceled;
                    if (ticket > 0 && ticketApp.UpdateWorkingOnStatus(ticket, user, status))
                        Response.Write(ResponseMessage.GetResponse(true));
                    else
                        Response.Write(ResponseMessage.GetResponse(false, ticketApp.FirstBrokenRuleMessageContent));
                    break;
                case "setworkingonnone":
                    int.TryParse(Request.Params["ticket"], out ticket);
                    status = TicketUserStatus.None;
                    if (ticket > 0 && ticketApp.UpdateWorkingOnStatus(ticket, user, status))
                        Response.Write(ResponseMessage.GetResponse(true));
                    else
                        Response.Write(ResponseMessage.GetResponse(false, ticketApp.FirstBrokenRuleMessageContent));
                    break;
                case "set_client_notification_on":
                    int.TryParse(Request.Params["ticket"], out ticket);
                    int.TryParse(Request.Params["user"], out targetUser);
                    if (ticket < 1 || targetUser < 1)
                    {
                        Response.Write(ResponseMessage.GetResponse(false, "Arguments error."));
                        break;
                    }
                    if (!int.TryParse(Request.Params["update"], out update) || update < 1)
                    {
                        if (ticketApp.IsTicketUser(ticket, UserID, TicketUsersType.Create, TicketUsersType.PM))
                        {
                            flag = ticketApp.AssignUsers(ticket, TicketUsersType.Client, targetUser);
                            if (flag)
                            {
                                new SendHandler(
                                    () => ticketStatusApp.SendEmailtoClientForFeedBack(ticket, targetUser))
                                    .BeginInvoke(null, null);
                            }
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    if (!flag)
                    {
                        Response.Write(ResponseMessage.GetResponse(false, ticketApp.FirstBrokenRuleMessageContent));
                        break;
                    }
                    //flag = ticketApp.UpdateNotification(ticket, targetUser);   // 更改状态去除气泡 提示
                    //if (!flag)
                    //{
                    //    Response.Write(ResponseMessage.GetResponse(false, ticketApp.FirstBrokenRuleMessageContent));
                    //    break;
                    //}

                    flag = ticketApp.UpdateTicketStatus(ticket, UserTicketStatus.WaitClientFeedback, targetUser);
                    if (!flag)
                    {
                        Response.Write(ResponseMessage.GetResponse(false, ticketApp.FirstBrokenRuleMessageContent));
                        break;
                    }
                    if (UserInfo.Role == RolesEnum.PM)
                        ticketApp.UpdateTicketStatus(ticket, UserTicketStatus.WaitClientFeedback, UserID);
                    new SendHandler(() => ticketStatusApp.SendEmailtoClientForFeedBack(ticket, targetUser)).BeginInvoke(null, null);

                    Response.Write(ResponseMessage.GetResponse(true));
                    break;
                case "set_client_notification_off":
                    int.TryParse(Request.Params["ticket"], out ticket);
                    int.TryParse(Request.Params["user"], out targetUser);
                    if (ticket < 1 || targetUser < 1)
                    {
                        Response.Write(ResponseMessage.GetResponse(false, "Arguments error."));
                        break;
                    }
                    if (!int.TryParse(Request.Params["update"], out update) || update < 1)
                    {
                        Response.Write(ResponseMessage.GetResponse(true));
                        break;
                    }
                    else
                    {
                        flag = true;
                    }
                    if (!flag)
                    {
                        Response.Write(ResponseMessage.GetResponse(false, ticketApp.FirstBrokenRuleMessageContent));
                        break;
                    }

                    ticketApp.UpdateTicketStatus(ticket, UserTicketStatus.Normal, targetUser);
                    flag = ticketApp.DeleteUserFromTicket(ticket, targetUser);
                    if (!flag)
                    {
                        Response.Write(ResponseMessage.GetResponse(false, ticketApp.FirstBrokenRuleMessageContent));
                        break;
                    }
                    Response.Write(ResponseMessage.GetResponse(true));
                    break;
                default:
                    Response.Write("[]");
                    break;
            }
        }
        private delegate void SendHandler();

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}