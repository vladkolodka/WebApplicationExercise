using System.Web.Mvc;
using NLog;

namespace WebApplicationExercise.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            LogManager.GetCurrentClassLogger().Log(LogLevel.Info, "test test test");
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}