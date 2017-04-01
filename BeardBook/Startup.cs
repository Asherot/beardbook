using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BeardBook.Startup))]
namespace BeardBook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = SimpleInjectorConfig.Initialize(app);
            ConfigureAuth(app, container);
        }
    }
}
