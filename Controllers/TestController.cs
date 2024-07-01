using Microsoft.AspNetCore.Mvc;
using YouCan.Models;

namespace YouCan.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Main()
        {
            return View();
        }
        public IActionResult CreateTest()
        {
            return View();
        }
        public IActionResult TakeTest()
        {
            return View();
        }
    }
}
