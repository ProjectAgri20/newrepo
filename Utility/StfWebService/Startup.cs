using System.Web.Http;
using HP.ScalableTest.Service.StfWebService;
using Microsoft.Owin;
using Owin;


[assembly: OwinStartup(typeof(Startup))]

namespace HP.ScalableTest.Service.StfWebService
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}"
            );

            appBuilder.UseWebApi(config);

        }
    }
}
