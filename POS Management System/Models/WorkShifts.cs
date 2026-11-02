namespace POS_Management_System.Models
{
    public class WorkShifts
    {
        public int Id { get; set; }
        public string ShiftName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
