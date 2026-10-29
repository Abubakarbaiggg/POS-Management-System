using System.ComponentModel.DataAnnotations;

namespace POS_Management_System.Models
{
    public class CashierBreak
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        public int BreakId { get; set; }
        public virtual Break? Break { get; set; }
        public int CashierAttendanceId { get; set; }
        public virtual CashierAttendance? CashierAttandance{ get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }   
}
