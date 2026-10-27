namespace POS_Management_System.Models
{
    public class CashierAttendance
    {
        public int Id { get; set; }
        public DateTime ClockIn { get; set; }
        public DateTime ClockOut { get; set; }
        public int BreakMinutes { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; } = AttendanceStatus.Scheduled;
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
