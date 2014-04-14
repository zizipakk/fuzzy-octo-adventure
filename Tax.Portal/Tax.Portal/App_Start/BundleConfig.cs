using System.Globalization;
using System.Web;
using System.Web.Optimization;


namespace Links
{
    public static partial class bundles
    {
        public static partial class scripts
        {
            public static readonly string jquery = "~/Scripts/jquery";
            public static readonly string jqueryval = "~/Scripts/jqueryval";
            public static readonly string bootstrap = "~/Scripts/bootstrap";
            public static readonly string modernizr = "~/Scripts/modernizr";
            public static readonly string jqgrid = "~/Scripts/jqgrid";
            //nem megy
            //public static readonly string ckeditorjs = "~/Scripts/ckeditorjs";
        }
        public static partial class styles
        {
            public static readonly string css = "~/Content/css";
            public static readonly string theme = "~/Content/themes/base/css";
            //nem megy
            //public static readonly string ckeditorcss = "~/Scripts/ckeditorcss";
        }
    }
}

namespace Tax.Portal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(Links.bundles.scripts.jquery).Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle(Links.bundles.scripts.jqueryval).Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle(Links.bundles.scripts.modernizr).Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle(Links.bundles.scripts.bootstrap).Include(
                    //string.Format(CultureInfo.CurrentCulture, "~{0}", Links.Scripts.bootstrap_js)
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle(Links.bundles.scripts.jqgrid).Include(
                        "~/Scripts/jquery.blockUI.min.js", 
                        "~/Scripts/i18n/grid.locale-hu.js", 
                        "~/Scripts/jquery.jqGrid.min.js", 
                        "~/Scripts/jquery-ui-1.10.3.js",
                        "~/Scripts/jquery.ui.datepicker-hu.js"
            ));

            bundles.Add(new StyleBundle(Links.bundles.styles.css).Include(
                        "~/Content/bootstrap.css",    
                        "~/Content/font-awesome.css",
                        "~/Content/jquery.jqGrid/ui.jqgrid.css"
            ));

            bundles.Add(new StyleBundle(Links.bundles.styles.theme).Include(
                        "~/Content/themes/base/jquery-ui.css"));

//nem megy abundlival
            //bundles.Add(new StyleBundle(Links.bundles.styles.ckeditorcss).Include(
            //            "~/Scripts/ckeditor/contents.css"
            //));

            //bundles.Add(new ScriptBundle(Links.bundles.scripts.ckeditorjs).Include(
            //            "~/Scripts/ckeditor/ckeditor.js"
            //));
        
        }
    }
}
