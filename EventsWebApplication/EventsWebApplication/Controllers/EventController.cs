using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
