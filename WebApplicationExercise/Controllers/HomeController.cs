namespace WebApplicationExercise.Controllers
{
    using System.Web.Mvc;

    using NLog;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            LogManager.GetCurrentClassLogger().Log(LogLevel.Info, "test test test");
            this.ViewBag.Title = "Home Page";

            return this.View();
        }
    }
}