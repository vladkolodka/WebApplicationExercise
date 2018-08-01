namespace WebApplicationExercise.Core
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    using Autofac;
    using Autofac.Extras.NLog;
    using Autofac.Integration.WebApi;

    using AutoMapper;

    using WebApplicationExercise.Managers;
    using WebApplicationExercise.Managers.Interfaces;
    using WebApplicationExercise.Services;
    using WebApplicationExercise.Services.Interfaces;

    public static class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
            
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic).As<Profile>();

            builder.Register(
                c =>
                    {
                        var config = new MapperConfiguration(
                            cfg =>
                                {
                                    foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                                    {
                                        cfg.AddProfile(profile);
                                    }
                                });

                        config.AssertConfigurationIsValid();

                        return config;
                    }).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MainDataContext>().AsSelf().InstancePerRequest();

            builder.RegisterType<CustomerManager>().As<ICustomerManager>();
            builder.RegisterType<OrderService>().As<IOrderService>();
            builder.RegisterType<CurrencyConverterService>().As<ICurrencyConverterService>().SingleInstance();

            builder.RegisterModule<NLogModule>();

           GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());
        }
    }
}