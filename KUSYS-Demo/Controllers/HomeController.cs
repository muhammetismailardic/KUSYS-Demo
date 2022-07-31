using KUSYS_Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KUSYS_Demo.Controllers
{
    [Authorize(Roles = "Administrator,Instructor")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
      
    }
}