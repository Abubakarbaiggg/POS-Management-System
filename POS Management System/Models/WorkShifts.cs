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
        public Boolean IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<WorkShiftsCashier> Cashiers { get; set; } = new List<WorkShiftsCashier>();
    }
}
