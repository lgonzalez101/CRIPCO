using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRIPCO.Startup))]
namespace CRIPCO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
