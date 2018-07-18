using System.Web;
using System.Web.Http;
using System.Web.Http.Tracing;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplicationExercise.Core;
using WebApplicationExercise.Services.Logging;

namespace WebApplicationExercise
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new NLogger());

            AutofacConfig.ConfigureContainer();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}