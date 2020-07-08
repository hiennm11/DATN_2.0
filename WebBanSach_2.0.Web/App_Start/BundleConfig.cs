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
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryex").Include(
                        "~/Scripts/umd/popper.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.toast.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                        "~/Scripts/ckeditor/ckeditor.js"));

            bundles.Add(new ScriptBundle("~/bundles/chosen").Include(
                        "~/Scripts/chosen/chosen.jquery.min.js",
                        "~/Scripts/chosen/chosen.jquery.js",
                        "~/Scripts/chosen/docsupport/prism.js",
                        "~/Scripts/chosen/docsupport/init.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/normalize.min.css",
                      "~/Content/site.css",
                      "~/Content/fontawesome-free/css/all.min.css",
                      "~/Content/jquery.toast.css"));

            //Admin style n script
            bundles.Add(new StyleBundle("~/bundles/admin/css").Include(
                "~/Areas/Admin/Content/css/sb-admin.css",
                "~/Areas/Admin/Content/vendor/datatables/dataTables.bootstrap4.css",
                "~/Areas/Admin/Content/vendor/fontawesome-free/css/all.min.css",
                "~/Areas/Admin/Content/css/bs-pagination.css",
                "~/Areas/Admin/Content/css/jquery-ui.css",
                "~/Scripts/chosen/chosen.css"));

            bundles.Add(new ScriptBundle("~/bundles/admin/js").Include(
                "~/Areas/Admin/Content/vendor/bootstrap/js/bootstrap.bundle.min.js",
                "~/Areas/Admin/Content/vendor/jquery-easing/jquery.easing.min.js",
                "~/Areas/Admin/Content/vendor/chart.js/Chart.min.js",
                "~/Areas/Admin/Content/vendor/datatables/jquery.dataTables.js",
                "~/Areas/Admin/Content/vendor/datatables/dataTables.bootstrap4.js",
                "~/Areas/Admin/Content/js/sb-admin.min.js",
                "~/Areas/Admin/Content/vendor/jquery/jquery-ui.js",
                "~/Areas/Admin/Content/js/adminApp.js"));
           
            bundles.Add(new ScriptBundle("~/bundles/admin/order-admin").Include(
                "~/Areas/Admin/Content/js/order-admin.js"));

            bundles.Add(new ScriptBundle("~/bundles/app-js.js").Include(
                "~/Scripts/app.js"));

        }
    }
}
