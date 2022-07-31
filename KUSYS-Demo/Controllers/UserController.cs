using Microsoft.AspNetCore.Mvc;

namespace KUSYS_Demo.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
