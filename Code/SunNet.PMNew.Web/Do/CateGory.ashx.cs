using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Utils;
using StructureMap;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CateGory : IHttpHandler
    {
        public int UserID
        {
            get
            {
                return IdentityContext.UserID;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            CateGoryApplication ccApp = new CateGoryApplication();
            HttpRequest request = context.Request;
            context.Response.ContentType = "text/plain";
            if (IdentityContext.UserID <= 0)
            {
                context.Response.Write("[]");
                return;
            }
            if (string.IsNullOrEmpty(request["type"]))
            {
                List<CateGoryEntity> listcategories = ccApp.GetCateGroyListByUserID(UserID);
                if (listcategories == null || listcategories.Count == 0)
                {
                    context.Response.Write("[]");
                }
                else
                {
                    string jsonCategories = UtilFactory.Helpers.JSONHelper.GetJson<List<CateGoryEntity>>(listcategories);
                    context.Response.Write(jsonCategories);
                }
            }
            else
            {
                int ticketid = 0;
                int categoryid = 0;
                bool checkinput = (
                            (!string.IsNullOrEmpty(request["ticketid"]))
                            && (!string.IsNullOrEmpty(request["cagetoryid"]))
                            && int.TryParse(request["ticketid"], out ticketid)
                            && int.TryParse(request["cagetoryid"], out categoryid)
                            && ticketid >= 0
                            && categoryid > 0);
                if (checkinput)
                {
                    CateGoryTicketEntity model = CateGoryFactory.CreateCateGoryTicketEntity(UserID, ObjectFactory.GetInstance<ISystemDateTime>());
                    model.TicketID = ticketid;
                    model.CategoryID = categoryid;
                    switch (request["type"])
                    {
                        case "addtocategory":
                            int id = ccApp.AssignTicketToCateGory(model);
                            if (id > 0)
                            {
                                context.Response.Write("true");
                            }
                            else
                            {
                                context.Response.Write("false");
                            }
                            break;
                        case "removefromcategory":
                            bool result = ccApp.RemoveTicketFromCateGory(ticketid, categoryid);
                            context.Response.Write(result.ToString().ToLower());
                            break;
                        case "deletecategory":
                            bool result2 = ccApp.DeleteCateGroy(categoryid);
                            context.Response.Write(result2.ToString().ToLower());
                            break;
                        default:
                            context.Response.Write("null");
                            break;
                    }
                }
                else
                {
                    context.Response.Write("false");
                }
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
