using System.Web.Mvc;

namespace PM.Document.Web.Areas.Comment
{
    public class CommentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Comment";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Comment_default",
                "Comment/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
