using DocumentFormat.OpenXml.Office2010.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Models;
using System.Security.Claims;

namespace POS_Management_System.Controllers
{
    public class CashierAttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CashierAttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var breaks = await _context.Breaks.ToListAsync();
            return View(breaks);
        }

        public IActionResult CashierReply()
        {
            return View();
        }
        public async Task<IActionResult> AddBreak(string BreakType)
        {
            if (string.IsNullOrWhiteSpace(BreakType))
            {
                TempData["error"] = "Please select Break";
            }
            TempData["success"] = $"{BreakType} started successfuly.";
            return RedirectToAction("Index", "CashierAttendance");
        }

        public async Task<IActionResult> PunchIn()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }

            var today = DateTime.Today;
            var checkClockIn = await _context.CashierAttendances
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.ClockIn >= today &&
                    x.ClockIn < today.AddDays(1));

            if (checkClockIn)
            {
                TempData["error"] = "You have already punched in today.";
                return RedirectToAction("Index", "CashierAttendance");
            }

            _context.CashierAttendances.Add(new CashierAttendance
            {
                UserId = userId,
                ClockIn = DateTime.Now,
                AttendanceStatus = AttendanceStatus.Present
            });

            await _context.SaveChangesAsync();
            TempData["success"] = "Punch in successful.";
            return RedirectToAction("Index", "CashierAttendance");
        }
        public async Task<IActionResult> PunchOut()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }
            var today = DateTime.Today;
            var attendance = await _context.CashierAttendances
               .Where(x =>
                   x.UserId == userId &&
                   x.ClockIn >= today &&
                   x.ClockIn < today.AddDays(1)).OrderByDescending(x => x.ClockIn).FirstOrDefaultAsync();

            if (attendance == null)
            {
                TempData["error"] = "Please punch in first.";
                return RedirectToAction("Index");
            }

            if (attendance.ClockOut != null)
            {
                TempData["error"] = "You have already punched out.";
                return RedirectToAction("Index");
            }
            attendance.ClockOut = DateTime.Now;
            attendance.AttendanceStatus = AttendanceStatus.Completed;
            await _context.SaveChangesAsync();
            TempData["success"] = "Punch out successfuly.";
            return RedirectToAction("Index", "CashierAttendance");
        }

        public async Task<IActionResult> Ready()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }
            var today = DateTime.Today;
            var attendance = await _context.CashierAttendances
                          .Where(x =>
                              x.UserId == userId &&
                              x.ClockIn >= today &&
                              x.ClockIn < today.AddDays(1)).OrderByDescending(x => x.ClockIn).FirstOrDefaultAsync();

            if (attendance == null)
            {
                TempData["error"] = "Please punch in first.";
                return RedirectToAction("Index");
            }



            TempData["success"] = "Ready successfuly.";
            return RedirectToAction("Index", "CashierAttendance");
        }
    }
}
