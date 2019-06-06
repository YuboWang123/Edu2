using System.Web;
using System.Web.Optimization;

namespace Edu.UI
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js", "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"
                     ));

            //app func
            bundles.Add(new StyleBundle("~/Content/AppCss").Include(
                 "~/Content/site.css"
                 ));

            bundles.Add(new ScriptBundle("~/bundles/AppJs").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/app.js"
            ));
            //

            bundles.Add(new StyleBundle("~/Content/AdminCss").Include(
                    "~/area/console/public/css/layout/style.css",
                     "~/area/console/public/css/layout/dermadefault.css",
                     "~/area/console/public/css/layout/templatecss.css"
                   ));
            //not used yet.
            bundles.Add(new StyleBundle("~/Content/SchoolCss").Include(
                 "~/area/Shool/public/css/layout/style.css",
                  "~/area/School/public/css/layout/dermadefault.css",
                  "~/area/School/public/css/layout/templatecss.css"
                ));
        }
    }
}
