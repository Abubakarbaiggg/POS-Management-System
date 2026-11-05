using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            var shifts = await _context.WorkShifts.Include(s => s.Cashiers).ThenInclude(c => c.User).ToListAsync();
            return View(shifts);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Cashiers = await _userManager.GetUsersInRoleAsync("Cashier");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkShifts model,List<string> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                ModelState.AddModelError("", "Please select at least one cashier.");
            }
            var cashiers = await _userManager.GetUsersInRoleAsync("Cashier");

            var validUserIds = cashiers.Where(u => userIds != null && userIds.Contains(u.Id))
                                .Select(u => u.Id).Distinct().ToList();

            if(!ModelState.IsValid)
            {
                ViewBag.Cashiers = cashiers;
                return View(model);
            }
            var shift = new WorkShifts
            {
                ShiftName = model.ShiftName,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                IsActive = true,
                CreatedAt = DateTime.Now
            };  

            foreach(var userId in validUserIds)
            {
                shift.Cashiers.Add(new WorkShiftsCashier
                {
                    UserId = userId
                });
            }

            _context.WorkShifts.Add(shift);
    
            await _context.SaveChangesAsync();
            TempData["Success"] = "Shift assigned to selected cashiers successfully.";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var workshift = await _context.WorkShifts.Include(s => s.Cashiers).FirstOrDefaultAsync(s => s.Id == id);
            if(workshift == null)
            {
                return NotFound();
            }
            ViewBag.Cashiers = await _userManager.GetUsersInRoleAsync("Cashier");
            ViewBag.SelectedCashiers = workshift.Cashiers.Select(c => c.UserId).ToList();
            return View(workshift);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,WorkShifts model, List<string> userIds)
        {
            var workshift = await _context.WorkShifts.Include(s => s.Cashiers).FirstOrDefaultAsync(c => c.Id == id);
            if(workshift == null)
            {
                return NotFound();
            }
            if (userIds == null || !userIds.Any())
            {
                ModelState.AddModelError("", "Please select at least one cashier.");
            }
            var cashiers = await _userManager.GetUsersInRoleAsync("Cashier");

            var validUserIds = cashiers.Where(u => userIds != null && userIds.Contains(u.Id))
                                .Select(u => u.Id).Distinct().ToList();

            if (!ModelState.IsValid)
            {
                ViewBag.Cashiers = cashiers;
                return View(model);
            }

            workshift.ShiftName = model.ShiftName;
            workshift.StartDate = model.StartDate;
            workshift.EndDate = model.EndDate;
            workshift.StartTime = model.StartTime;
            workshift.EndTime = model.EndTime;
            workshift.Cashiers.Clear();

            foreach (var userId in validUserIds)
            {
                workshift.Cashiers.Add(new WorkShiftsCashier
                {
                    UserId = userId
                });
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Shift updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workShift = await _context.WorkShifts.Include(s => s.Cashiers).FirstOrDefaultAsync(s => s.Id == id);
            if (workShift != null)
            {
                return NotFound();
            }
            _context.WorkShiftsCashiers.RemoveRange(workShift.Cashiers);
            _context.WorkShifts.Remove(workShift);
            TempData["Success"] = "Shift deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

    }
}
