using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Z9TheCoffee.Startup))]
namespace Z9TheCoffee
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
