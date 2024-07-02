using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateAnalogyOrAddition()
        {
            return View();
        }
        public IActionResult TakeAnalogyOrAddition()
        {
            return View();
        }

    }
}
