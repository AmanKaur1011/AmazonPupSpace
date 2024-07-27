using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AmazonPupSpace.Startup))]
namespace AmazonPupSpace
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
