namespace WebApplicationExercise
{
    using System.Web.Http;

    using WebApplicationExercise.Filters;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/v1/{controller}/{id}", new { id = RouteParameter.Optional });

            config.Filters.Add(new ExecutionTimeFilterAttribute());
        }
    }
}