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
        }
        public static partial class styles
        {
            public static readonly string css = "~/Content/css";
            public static readonly string theme = "~/Content/themes/base/css";
        }
    }
}

namespace Tax.Portal
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(Links.bundles.scripts.jquery).Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle(Links.bundles.scripts.jqueryval).Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle(Links.bundles.scripts.modernizr).Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle(Links.bundles.scripts.bootstrap).Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle(Links.bundles.scripts.jqgrid).Include(
                        "~/Scripts/jquery.blockUI.min.js", 
                        "~/Scripts/i18n/grid.locale-en.js", 
                        "~/Scripts/jquery.jqGrid.min.js", 
                        "~/Scripts/jquery-ui-1.10.3.js",
                        "~/Scripts/jquery.ui.datepicker-en.js"
            ));

            bundles.Add(new StyleBundle(Links.bundles.styles.css).Include(
                        "~/Content/bootstrap.css",    
                        "~/Content/font-awesome.css",
                        "~/Content/jquery.jqGrid/ui.jqgrid.css"
            ));

            bundles.Add(new StyleBundle(Links.bundles.styles.theme).Include(
                        "~/Content/themes/base/jquery-ui.css"));
        
        }
    }
}
