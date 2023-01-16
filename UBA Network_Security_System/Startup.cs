using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UBA_Network_Security_System.Startup))]
namespace UBA_Network_Security_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
