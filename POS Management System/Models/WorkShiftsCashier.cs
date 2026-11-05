namespace POS_Management_System.Models
{
    public class WorkShiftsCashier
    {
        public int Id { get; set; }
        public int WorkShiftId { get; set; }
        public virtual WorkShifts WorkShift { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
