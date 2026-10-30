using DocumentFormat.OpenXml.Office2010.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS_Management_System.Data;
using POS_Management_System.Models;
using POS_Management_System.Models.ViewModels;
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }
            var today = DateTime.Today;
            var attendance = await _context.CashierAttendances.Include(x => x.CashierBreaks)
                .Where(x =>
                    x.UserId == userId &&
                    x.ClockIn >= today &&
                    x.ClockIn < today.AddDays(1)).OrderByDescending(x => x.ClockIn).FirstOrDefaultAsync();

            var breaks = await _context.Breaks.ToListAsync();
            var viewModal = new CashierAttendanceViewModal
            {
                Breaks = breaks,
                Attendance = attendance,
                ActiveBreakStart = attendance?.CashierBreaks.FirstOrDefault(x => x.End == null)?.Start,
                CompletedBreakSeconds = attendance?.CashierBreaks.Where(x => x.Start != null && x.End != null)
                    .Sum(x => (x.End!.Value - x.Start!.Value).TotalSeconds) ?? 0
                };
            return View(viewModal);
        }

        public IActionResult CashierReply()
        {
            return View();
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBreak(int BreakType)
        {
            if (BreakType <= 0)
            {
                TempData["error"] = "Please select Break";
                return RedirectToAction("Index");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }
            var today = DateTime.Today;
            var attendance = await _context.CashierAttendances.Where(x =>
                x.UserId == userId &&
                x.ClockIn >= today &&
                x.ClockIn < today.AddDays(1))
                .OrderByDescending(x => x.ClockIn).FirstOrDefaultAsync();

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
            var selectedBreak = await _context.Breaks.FirstOrDefaultAsync(x => x.Id == BreakType);

            if (selectedBreak == null)
            {
                TempData["error"] = "Invalid break selected.";
                return RedirectToAction("Index");
            }
            var activeBreak = await _context.CashierBreaks.AnyAsync(x => x.CashierAttendanceId == attendance.Id && x.End == null);
            if (activeBreak)
            {
                TempData["error"] = "Please finish your current break first.";
                return RedirectToAction("Index");
            }
            _context.CashierBreaks.Add(new CashierBreak
            {
                UserId = userId,
                BreakId = selectedBreak.Id,
                CashierAttendanceId = attendance.Id,
                Start = DateTime.Now
            });
            await _context.SaveChangesAsync();
            TempData["success"] = $"{selectedBreak.Name} started successfuly.";
            return RedirectToAction("Index", "CashierAttendance");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            if (attendance.ClockOut != null)
            {
                TempData["error"] = "You have already punched out.";
                return RedirectToAction("Index");
            }
            var activeBreak = await _context.CashierBreaks.FirstOrDefaultAsync(x => x.CashierAttendanceId == attendance.Id && x.End == null);
            if (activeBreak == null)
            {
                TempData["error"] = "You are not on a break.";
                return RedirectToAction("Index");
            }
            activeBreak.End = DateTime.Now;
            attendance.AttendanceStatus = AttendanceStatus.Present;
            await _context.SaveChangesAsync();
            TempData["success"] = "You are ready to work.";
            return RedirectToAction("Index", "CashierAttendance");
        }
    }
}
