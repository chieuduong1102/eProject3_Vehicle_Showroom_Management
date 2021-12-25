using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eProject3_Vehicle_Showroom_Management.Startup))]
namespace eProject3_Vehicle_Showroom_Management
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
