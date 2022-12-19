using Microsoft.AspNetCore.Mvc;

namespace PMS.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
