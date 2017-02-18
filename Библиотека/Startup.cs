using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Библиотека.Startup))]
namespace Библиотека
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
