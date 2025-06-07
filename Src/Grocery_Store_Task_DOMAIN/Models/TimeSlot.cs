using System.ComponentModel.DataAnnotations;

namespace Grocery_Store_Task_DOMAIN.Models
{
    public class TimeSlot
    {
        [Key]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get { return StartDate.AddHours(1); } }
        public bool IsGreen { get; set; }

        public virtual IEnumerable<Cart>? SlotProducts { get; set; }
    }
}
