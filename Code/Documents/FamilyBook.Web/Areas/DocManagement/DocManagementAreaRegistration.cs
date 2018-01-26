using System.Web.Mvc;

namespace PM.Document.Web.Areas.DocManagement
{
    public class DocManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DocManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var defaults = new { action = "Index", id = UrlParameter.Optional };
            context.MapRoute(
                "DocManagement_default",
                "DocManagement/{controller}/{action}/{id}", defaults);
        }
    }
}
