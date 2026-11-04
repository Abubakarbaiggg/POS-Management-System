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
        public async Task<IActionResult> Create(WorkShifts model, [FromForm(Name = "UserIds")] List<string> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                ModelState.AddModelError("", "Please select at least one cashier.");
            }

            if (!ModelState.IsValid)
            {   
                ViewBag.Cashiers = await _userManager.GetUsersInRoleAsync("Cashier");
                return View(model);
            }

            foreach (var userId in userIds)
            {
                var shift = new WorkShifts
                {
                    UserId = userId,
                    ShiftName = model.ShiftName,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };  

                _context.WorkShifts.Add(shift);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Shift assigned to selected cashiers successfully.";
            return RedirectToAction("Index");
        }
        public IActionResult Edit()
        {
            return View();
        }

    }
}
