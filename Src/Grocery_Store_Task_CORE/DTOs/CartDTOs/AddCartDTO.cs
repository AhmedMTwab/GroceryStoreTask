namespace Grocery_Store_Task_CORE.DTOs.CartDTOs
{
    public class AddCartDTO
    {
        public DateTime StartDate { get; set; }
        public bool IsGreen { get; set; }

        public virtual IEnumerable<Guid> CartProductsIds { get; set; } = default!;
    }
}
