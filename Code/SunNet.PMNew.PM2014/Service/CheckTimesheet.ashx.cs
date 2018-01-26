using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// Summary description for CheckTimesheet
    /// </summary>
    public class CheckTimesheet : DoBase ,IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            TimeSheetApplication app = new TimeSheetApplication();
            context.Response.ContentType = "text/plain";

            DateTime startDate =DateTime.Now;
            DateTime.TryParse(context.Request.QueryString["startDate"] + "", out startDate);

            DateTime endDate =DateTime.Now ;
            DateTime.TryParse(context.Request.QueryString["endDate"] + "", out endDate);

            if(endDate > DateTime.Now)
                endDate = DateTime.Now;

            List<CheckTimesheetEntity> list = app.GetTimesheetList(startDate, endDate);

            if(list != null && list.Count >0)
                context.Response.Write(JsonConvert.SerializeObject(list, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" }));
            else
                context.Response.Write("[]");
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