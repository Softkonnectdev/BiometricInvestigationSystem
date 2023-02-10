using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CrimeInvestigationSystem.Startup))]
namespace CrimeInvestigationSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
