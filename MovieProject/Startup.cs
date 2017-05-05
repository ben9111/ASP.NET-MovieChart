using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieProject.Startup))]
namespace MovieProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
