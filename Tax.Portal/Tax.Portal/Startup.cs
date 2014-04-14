using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tax.Portal.Startup))]
namespace Tax.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
