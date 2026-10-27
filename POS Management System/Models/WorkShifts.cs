namespace POS_Management_System.Models
{
    public class WorkShifts
    {
        public int Id { get; set; }
        public string? ShiftName{ get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public Boolean IsActive { get; set; }
    }
}
