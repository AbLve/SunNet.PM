using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Web.Do;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// Email 的摘要说明
    /// </summary>
    public class Email : DoBase, IHttpHandler
    {
        private UserApplication userApp = new UserApplication();

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
            string action = context.Request.Params["action"] ?? "";
            action = action.ToLower();
            int user = 0;
            switch (action)
            {
                case "sendemail":
                    if (int.TryParse(Request.Params["user"], out user) && user > 0)
                    {
                        var u = userApp.GetUser(user);
                        string contentTemplete = FileHelper.GetEmailTemplate("SendEmailFromUserList.txt");
                        string from = Config.DefaultSendEmail;
                        string to = u.Email;
                        string subject = string.Format("Notice - Project Management - SunNet Solutions");
                        string content = string.Empty;
                        if (!string.IsNullOrEmpty(contentTemplete.Trim()))
                        {
                            content = contentTemplete.Trim().Replace("{Firstname}", u.FirstName)
                                                            .Replace("{Lastname}", u.LastName)
                                                            .Replace("{EmailDate}", DateTime.Now.ToString("MM/dd/yyyy"));
                        }
                        IEmailSender sender = ObjectFactory.GetInstance<IEmailSender>();
                        sender.SendMail(to, from, subject, content);
                        Response.Write(ResponseMessage.GetResponse(true));
                    }
                    else
                        Response.Write("[]");
                    break;
                default:
                    Response.Write("[]");
                    break;
            }
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