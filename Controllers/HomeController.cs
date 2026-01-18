using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.ProjectName = "Task Manager App";
            return View();
        }

        public IActionResult Error()
        {
            ViewBag.ProjectName = "Task Manager App";
            return View();
        }
        public IActionResult StatusCode(int code)
        {
            ViewBag.ProjectName = "Task Manager App";
            ViewBag.StatusCode = code;
            return View();
        }

    }
}
