using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Web.Do;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.TicketModel.UserTicket;
using System.Data;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// Ticket 的摘要说明
    /// </summary>
    public class Ticket : DoBase, IHttpHandler
    {
        FeedBackApplication fbAPP = new FeedBackApplication();
        FileApplication fileApp = new FileApplication();
        TicketsApplication ticketApp = new TicketsApplication();
        UserApplication userApp = new UserApplication();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string action = context.Request.Params["action"].ToLower();
            var ticketID = 0;
            var count = 1;
            var star = 0;
            switch (action)
            {
                case "getfeedbacks":
                    int.TryParse(context.Request.Params["ticketID"], out ticketID);
                    int.TryParse(context.Request.Params["count"], out count);
                    var list = GetFeedbackByTicket(ticketID, count);

                    context.Response.Write(
                        JsonConvert.SerializeObject(
                        list.Select(x => new
                        {
                            text = context.Server.HtmlEncode(x.Description),
                            date = x.CreatedOn,
                            author = _userApp.GetUser(x.CreatedBy).GetClientUserName(UserInfo),
                            files = GetFeedbackFiles(x.FeedBackID)
                        }), DateConverter));
                    break;
                case "updatestar":
                    int.TryParse(context.Request.Params["ticketID"], out ticketID);
                    int.TryParse(context.Request.Params["star"], out star);
                    context.Response.Write(UpdateStar(ticketID, star).ToString().ToLower());
                    break;
                case "clearfeedbackmessage":
                    int.TryParse(context.Request.Params["ticketID"], out ticketID);
                    context.Response.Write(ClearFeedbackMessage(ticketID).ToString().ToLower());
                    break;
                case "chageisread":
                    int.TryParse(context.Request.Params["ticketID"], out ticketID);
                    context.Response.Write(ticketApp.UpdateIsRead(ticketID, TicketIsRead.Read).ToString().ToLower());
                    break;
                case "ticketstatus":
                    context.Response.Write(CacheTicketStatus(context.Request.Params["statusJson"]).ToString().ToLower());
                    break;
                case "getusertickets":
                    int currentUserId = int.Parse(context.Request.Params["currentuserid"]);
                    context.Response.Write(GetUserTickets(currentUserId));
                    break;
                default:
                    context.Response.Write("[]");
                    break;
            }
        }

        private object GetFeedbackFiles(int feedback)
        {
            var files = fileApp.GetFileListBySourceId(feedback, FileSourceType.FeedBack);
            return files.Select(x => new { id = x.FileID, title = x.FileTitle + x.ContentType });
        }

        private List<FeedBacksEntity> GetFeedbackByTicket(int ticketId, int count)
        {
            var list = fbAPP.GetFeedBackListByTicketId(ticketId, false,
                UserInfo.Role != RolesEnum.CLIENT && UserInfo.Role != RolesEnum.Contactor,
                UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN,
                UserInfo.Role == RolesEnum.Supervisor);
            if (list != null)
                list = list.FindAll(r => r.IsDelete == false);
            if (list == null)
                list = new List<FeedBacksEntity>();
            list = list.OrderByDescending(x => x.FeedBackID).Take(count).ToList();
            return list;
        }

        private bool UpdateStar(int ticketid, int star)
        {
            TicketsApplication tickApp = new TicketsApplication();
            return tickApp.UpdateTicketStar(ticketid, star);
        }

        private bool ClearFeedbackMessage(int ticketID)
        {
            TicketsApplication tickApp = new TicketsApplication();
            return tickApp.ClearNotification(ticketID, UserID);
        }

        private bool CacheTicketStatus(string statusJson = "[]")
        {
            DateTime local = DateTime.Now;
            string uniqueCacheKey = string.Format("TicketStatus_{0}_{1}", UserInfo.Role, local.ToString("yyyyMMdd"));
            CookieHelper helper = new CookieHelper();
            string cache = helper.Get(uniqueCacheKey);
            if (!string.IsNullOrEmpty(cache))
                helper.Remove(uniqueCacheKey);

            helper.Add(uniqueCacheKey, statusJson, local.AddDays(1));
            return true;
        }

        private string GetUserTickets(int currentUserId)
        {
            List<RolesEnum> roles = new List<RolesEnum>();
            roles.Add(RolesEnum.PM);
            roles.Add(RolesEnum.DEV);
            roles.Add(RolesEnum.QA);
            roles.Add(RolesEnum.Leader);
            HideUserEntity entity = userApp.GetHideUserByUserId(currentUserId);
            string hideUserIds = "";
            if (entity != null)
            {
                hideUserIds = entity.HideUserIds;
            }
            List<UserTicketModel> userTickets = userApp.SearchUserWithRole(roles, hideUserIds);
            string strUserTickets = JsonConvert.SerializeObject(userTickets);
            return strUserTickets;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}