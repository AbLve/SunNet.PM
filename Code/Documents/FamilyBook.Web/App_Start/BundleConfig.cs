using System.Web;
using System.Web.Optimization;

namespace PM.Document.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/global").Include(
                       "~/Scripts/global.js",
                       "~/Scripts/Sunnet/jquery.sunnet.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/v3.bootstrap/js/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload/js").Include(
                        "~/Scripts/uploadify/jquery.uploadify.js"));

            bundles.Add(new StyleBundle("~/bundles/upload/css").Include(
                        "~/Scripts/uploadify/uploadify.css"));

            // jQuery validate
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.ext.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //"~/Styles/rewritebootstrap.css",
            bundles.Add(new StyleBundle("~/bundles/basic").Include(
                "~/Styles/New2014_style/forms.css"));
            // document tree
            bundles.Add(new StyleBundle("~/bundles/dcoument/css").Include(
                           "~/Scripts/treeview/jquery.treeview.css"));
            bundles.Add(new ScriptBundle("~/bundles/documment/js").Include(
                        "~/Scripts/treeview/jquery.treeview.js",
                        "~/Scripts/treeview/jquery.contextmenu.r2.js"));

            // family tree
            bundles.Add(new ScriptBundle("~/bundles/tree/js").Include(
                        "~/Scripts/svgweb/src/svg.js",
                        "~/Scripts/svgweb/FamilyGraph.js"));
            bundles.Add(new StyleBundle("~/bundles/tree/css").Include(
               "~/Scripts/svgweb/tree.css"));

            // Index home page
            bundles.Add(new ScriptBundle("~/bundles/home/js").Include(
                        "~/Scripts/jquery-1.10.2.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.ext.js",
                        "~/Scripts/v3.bootstrap/js/bootstrap.js",
                        "~/Scripts/global.js"));
            bundles.Add(new StyleBundle("~/bundles/home/css").Include(
               "~/Scripts/v3.bootstrap/css/bootstrap.css",
               "~/Styles/home.css",
               "~/Styles/forms.css",
               "~/Styles/rewritebootstrap.css"));

            // knockout js
            bundles.Add(new ScriptBundle("~/bundles/js/knockout").Include(
                "~/Scripts/knockout-3.0.0.debug.js"));
        }
    }
}