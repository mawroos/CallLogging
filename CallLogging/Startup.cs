using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CallLogging.Startup))]
namespace CallLogging
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
