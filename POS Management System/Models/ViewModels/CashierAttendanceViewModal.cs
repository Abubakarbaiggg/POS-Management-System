namespace POS_Management_System.Models.ViewModels
{
    public class CashierAttendanceViewModal
    {
        public List<Break> Breaks { get; set; } = new();

        public CashierAttendance? Attendance { get; set; }

        public DateTime? ActiveBreakStart { get; set; }

        public double CompletedBreakSeconds { get; set; }
    }
}
