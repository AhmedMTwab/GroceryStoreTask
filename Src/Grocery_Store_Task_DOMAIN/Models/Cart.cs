using System.ComponentModel.DataAnnotations.Schema;

namespace Grocery_Store_Task_DOMAIN.Models
{
    public class Cart
    {
        public Guid Id { get; set; }

        [ForeignKey("TimeSlot")]
        public DateTime TimeSlotDate { get; set; }
        public virtual TimeSlot TimeSlot { get; set; } = default!;

        public virtual IEnumerable<Product> CartProducts { get; set; } = default!;
    }
}
