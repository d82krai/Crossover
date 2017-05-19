using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Crossover.app.Startup))]
namespace Crossover.app
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
