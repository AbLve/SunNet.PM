using SunNet.PMNew.App;
using SunNet.PMNew.Entity.EventModel;
using SunNet.PMNew.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for DoDeleteEvent
    /// </summary>
    public class DoDeleteEvent : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int eventID;
            if (int.TryParse(context.Request.Form["id"], out eventID))
            {
                IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                string strUserID = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("LoginUserID")));
                if (string.IsNullOrEmpty(strUserID))
                {
                    context.Response.Write("0");
                }
                int userID = int.Parse(strUserID);

                EventEntity entity = new EventsApplication().GetEventInfo(eventID);
                if (entity == null || entity.CreatedBy != userID)
                {
                    context.Response.Write("0");
                }

                if (new EventsApplication().Delete(eventID))
                {
                    context.Response.Write("1");
                }
                else
                {
                    context.Response.Write("0");
                }

            }
            else
            {
                context.Response.Write("0");
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