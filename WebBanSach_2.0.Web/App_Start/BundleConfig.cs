using System.Web;
using System.Web.Optimization;

namespace WebBanSach_2._0.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //Admin style n script
            bundles.Add(new StyleBundle("~/bundles/admin/css").Include(
                "~/Areas/Admin/Content/css/sb-admin.css",
                "~/Areas/Admin/Content/vendor/datatables/dataTables.bootstrap4.css",
                "~/Areas/Admin/Content/vendor/fontawesome-free/css/all.min.css",
                "~/Areas/Admin/Content/css/bs-pagination.css",
                "~/Areas/Admin/Content/css/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/admin/js").Include(
                "~/Areas/Admin/Content/vendor/bootstrap/js/bootstrap.bundle.min.js",
                "~/Areas/Admin/Content/vendor/jquery-easing/jquery.easing.min.js",
                "~/Areas/Admin/Content/vendor/chart.js/Chart.min.js",
                "~/Areas/Admin/Content/vendor/datatables/jquery.dataTables.js",
                "~/Areas/Admin/Content/vendor/datatables/dataTables.bootstrap4.js",
                "~/Areas/Admin/Content/js/sb-admin.min.js",
                "~/Areas/Admin/Content/vendor/jquery/jquery-ui.js",
                "~/Areas/Admin/Content/js/adminApp.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin/extensions/cate-admin").Include(               
                "~/Areas/Admin/Content/js/cate-admin.js"));
            bundles.Add(new ScriptBundle("~/bundles/admin/extensions/product-admin").Include(              
                "~/Areas/Admin/Content/js/product-admin.js"));
            bundles.Add(new ScriptBundle("~/bundles/admin/extensions/authord-admin").Include(
                "~/Areas/Admin/Content/js/authord-admin.js"));
            bundles.Add(new ScriptBundle("~/bundles/admin/extensions/authors-admin").Include(
                "~/Areas/Admin/Content/js/authors-admin.js"));

        }
    }
}
