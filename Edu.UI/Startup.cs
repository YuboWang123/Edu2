using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Edu.UI.Startup))]
namespace Edu.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
