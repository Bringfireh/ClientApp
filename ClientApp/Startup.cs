using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClientApp.Startup))]
namespace ClientApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
