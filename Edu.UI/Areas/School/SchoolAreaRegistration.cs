using System.Web.Mvc;

namespace Edu.UI.Areas.School
{
    public class SchoolAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "School";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "School_default",
                "School/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}