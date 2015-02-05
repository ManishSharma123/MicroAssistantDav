using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MicroAssistant.Startup))]
namespace MicroAssistant
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
