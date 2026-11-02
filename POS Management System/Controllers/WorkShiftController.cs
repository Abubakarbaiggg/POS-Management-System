using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using POS_Management_System.Data;
using POS_Management_System.Models;

namespace POS_Management_System.Controllers
{
    public class WorkShiftController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public WorkShiftController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Cashiers = await _userManager.GetUsersInRoleAsync("Cashier");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkShifts model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cashiers = await _userManager.GetUsersInRoleAsync("Cashier");
            }
            _context.WorkShifts.Add(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cashier shift created successfully";
            return RedirectToAction("Index");
        }
        public IActionResult Edit()
        {
            return View();
        }

    }
}
