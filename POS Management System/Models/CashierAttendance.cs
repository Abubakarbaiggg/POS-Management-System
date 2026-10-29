using System.ComponentModel.DataAnnotations;

namespace POS_Management_System.Models
{
    public class CashierAttendance
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; } = AttendanceStatus.Scheduled;
        public virtual ICollection<CashierBreak> CashierBreaks { get; set; } = new List<CashierBreak>();
    }
    public enum AttendanceStatus
    {
        Scheduled,
        Present,
        OnBreak,
        Absent,
        Late,
        Completed
    }
}
