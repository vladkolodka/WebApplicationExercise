using System.Web.Http;
using Autofac;
using Autofac.Extras.NLog;
using Autofac.Integration.WebApi;
using WebApplicationExercise.Services;
using WebApplicationExercise.Services.Interfaces;

namespace WebApplicationExercise.Core
{
    public static class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            #region register

            builder.RegisterType<MainDataContext>().AsSelf().InstancePerRequest();

            builder.RegisterType<OrderService>().As<IOrderService>();

            builder.RegisterModule<NLogModule>();

            #endregion

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());
        }
    }
}