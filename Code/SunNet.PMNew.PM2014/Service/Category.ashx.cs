using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Web.Do;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// Category 的摘要说明
    /// </summary>
    public class Category : DoBase, IHttpHandler
    {
        CateGoryApplication ccApp = new CateGoryApplication();

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            context.Response.ContentType = "application/json";

            string action = context.Request.Params["action"].ToLower();
            int ticketid = 0;
            int category = 0;
            switch (action)
            {
                case "removefromcategory":
                    int.TryParse(Request.Params["ticketid"], out ticketid);
                    int.TryParse(Request.Params["cagetory"], out category);
                    bool result = ccApp.RemoveTicketFromCateGory(ticketid, category);
                    context.Response.Write(result.ToString().ToLower());
                    break;
                case "addtocategory":
                    int.TryParse(Request.Params["ticketid"], out ticketid);
                    int.TryParse(Request.Params["cagetory"], out category);
                    CateGoryTicketEntity model = CateGoryFactory.CreateCateGoryTicketEntity(UserID, ObjectFactory.GetInstance<ISystemDateTime>());
                    model.TicketID = ticketid;
                    model.CategoryID = category;
                    int id = ccApp.AssignTicketToCateGory(model);
                    context.Response.Write(id > 0 ? "true" : "false");
                    break;
                default:
                    context.Response.Write("[]");
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