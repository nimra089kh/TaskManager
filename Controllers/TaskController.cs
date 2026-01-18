using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Data;
using TaskManager.Migrations;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {

        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public TaskController (AppDbContext context , UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string Searchstring , string Statusfilter)
        {
            ViewBag.ProjectName = "Task Manager App";
            var user = await _userManager.GetUserAsync(User);
            var tasks = _context.TaskItems.Where(t => t.userId == user.Id);


            ViewBag.SearchString = Searchstring;
            ViewBag.StatusList = new SelectList(
                new[]
                {
                    new { Value = "" , Text = "All"},
                    new { Value = "pending", Text = "Pending" },
                    new { Value = "completed", Text = "Completed" }
                },
                "Value",
                "Text",
                Statusfilter
                );

            if (!string.IsNullOrEmpty(Searchstring))
            {
                tasks = tasks.Where(t => t.Title.Contains(Searchstring) || t.Description.Contains(Searchstring));
            }

            if (!string.IsNullOrEmpty(Statusfilter))
            {
                if (Statusfilter == "pending")
                    tasks = tasks.Where(t => !t.IsCompleted);
                else if (Statusfilter == "completed")
                    tasks = tasks.Where(t => t.IsCompleted);

                tasks = tasks.OrderBy(t => t.DueDate);
                
            }
            return View(tasks.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ProjectName = "Task Manager App";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem task)
        {
            ViewBag.ProjectName = "Task Manager App";
            if (!ModelState.IsValid)
            {
                return View(task);

            }
            var user = await _userManager.GetUserAsync(User);
            task.userId = user.Id;

            _context.TaskItems.Add(task);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.ProjectName = "Task Manager App";

            var user = await _userManager.GetUserAsync(User);
            var task = _context.TaskItems.FirstOrDefault(t => t.Id == id && t.userId == user.Id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit (TaskItem task)
        {
            ViewBag.ProjectName = "Task Manager App";
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            _context.TaskItems.Update(task);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.ProjectName = "Task Manager App";
            var user = await _userManager.GetUserAsync(User);

            var task = _context.TaskItems.FirstOrDefault(t => t.Id == id && t.userId == user.Id);
            if(task == null)
            {
                return NotFound();
            }
            return View(task);
        }


        [HttpPost , ActionName("delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            ViewBag.ProjectName = "Task Manager App";

            var task = _context.TaskItems.FirstOrDefault(t => t.Id == id);
            if(task != null)
            {
                _context.Remove(task);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            ViewBag.ProjectName = "Task Manager App";
            var user = await _userManager.GetUserAsync(User);

            var task = _context.TaskItems.Find(id);
            if (task == null)
            {
                return NotFound();

            }
            task.IsCompleted = !task.IsCompleted;
            _context.SaveChanges();

            TempData["success"] = "Task status updated successfully.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.ProjectName = "Task Manager App";

            var user = await _userManager.GetUserAsync(User);

            var tasks = _context.TaskItems.Where(t => t.userId == user.Id).ToList();

            ViewBag.TotalTasks = tasks.Count;
            ViewBag.Completed = tasks.Count(t => t.IsCompleted);
            ViewBag.Pending = tasks.Count(t => !t.IsCompleted);
            ViewBag.OverDue = tasks.Count(t => !t.IsCompleted && t.DueDate < DateTime.Today);

            return View();

        }
    }
}

