using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartTruckerApp5.Startup))]
namespace SmartTruckerApp5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
