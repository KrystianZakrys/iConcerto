using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iConcerto.Startup))]
namespace iConcerto
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
