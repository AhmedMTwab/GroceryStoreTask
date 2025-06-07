using System.ComponentModel.DataAnnotations;
using Grocery_Store_Task_DOMAIN.Enums;

namespace Grocery_Store_Task_DOMAIN.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        [Length(minimumLength: 3, maximumLength: 50)]
        public string Name { get; set; } = default!;
        [MaxLength(200)]
        public string Description { get; set; } = default!;
        public ProductTypeEnum Category { get; set; } = default!;

        public virtual IEnumerable<Cart>? ProductCart { get; set; }



    }
}
