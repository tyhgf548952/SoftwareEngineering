using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(retry.Startup))]
namespace retry
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
