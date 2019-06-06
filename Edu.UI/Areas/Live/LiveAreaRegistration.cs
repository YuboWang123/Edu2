using System.Web.Mvc;

namespace Edu.UI.Areas.Live
{
    public class LiveAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Live";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Live_default",
                "Live/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}