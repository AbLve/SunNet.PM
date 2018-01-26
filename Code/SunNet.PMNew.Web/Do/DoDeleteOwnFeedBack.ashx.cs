using SunNet.PMNew.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for DoDeleteOwnFeedBack
    /// </summary>
    public class DoDeleteOwnFeedBack : DoBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            int feedbackID = QS(request.QueryString["FeedbackID"], 0);
            int userID = QS(request.QueryString["UID"], 0);
            if (userID != UserID)
            {
                response.Write("2"); //not autorized.
            }
            if (feedbackID == 0)
            {
                response.Write("3");
            }

            FeedBackApplication feedBackApplication = new FeedBackApplication();
            bool result = feedBackApplication.DeleteFeedback(feedbackID);
            if (result)
            {
                response.Write("1");
            }
            else
            {
                response.Write("0");
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