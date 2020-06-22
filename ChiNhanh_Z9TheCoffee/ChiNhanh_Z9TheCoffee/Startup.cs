using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChiNhanh_Z9TheCoffee.Startup))]
namespace ChiNhanh_Z9TheCoffee
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
