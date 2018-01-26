using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils.Providers;
using System.Timers;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Framework;
using System.IO;

namespace SunNet.PMNew.Web
{
    public class Global : System.Web.HttpApplication
    {

        string mailTemplatePath = string.Empty;

        protected void Application_Start(object sender, EventArgs e)
        {
            BootStrap.Config();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            for (int i = 0; i < Context.AllErrors.Length; i++)
            {
                WebLogAgent.Write(Context.AllErrors[i]);
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}