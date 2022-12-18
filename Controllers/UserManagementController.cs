using Microsoft.AspNetCore.Mvc;

namespace PMS.Controllers
{
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
