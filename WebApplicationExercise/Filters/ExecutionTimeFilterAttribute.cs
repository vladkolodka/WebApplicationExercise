namespace WebApplicationExercise.Filters
{
    using System.Diagnostics;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using NLog;

    /// <summary>
    ///     Time tracking for actions
    /// </summary>
    public class ExecutionTimeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            var actionInfo = actionExecutedContext.ActionContext.ActionDescriptor;

            var watch = (Stopwatch)actionExecutedContext.Request.Properties[actionInfo.ActionName];

            LogManager.GetCurrentClassLogger().Info(
                $"Action {actionInfo.ControllerDescriptor.ControllerName}.{actionInfo.ActionName} executed. Elapsed time: {watch.Elapsed}");
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            actionContext.Request.Properties[actionContext.ActionDescriptor.ActionName] = Stopwatch.StartNew();
        }
    }
}