using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestoWebApp.Startup))]
namespace RestoWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
