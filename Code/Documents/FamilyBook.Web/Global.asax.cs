using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Timers;
using FamilyBook.Common;
using SF.Framework.Log;
using FamilyBook.Business.Global;
using SunNet.PMNew.App;

namespace PM.Document.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            SFConfig.Init();
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            ConfigLog4net();
            GlobalDataBusiness.SetGolbalData();

            BootStrap.Config();

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            new log4netProvider().Log(Context.AllErrors);
        }

        private void ConfigLog4net()
        {
            string l4net = Server.MapPath("~/log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(l4net));
        }
    }
}