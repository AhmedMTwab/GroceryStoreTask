namespace Grocery_Store_Task_CORE.DTOs.ProductDTOs
{
    public class GetProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public String Category { get; set; } = default!;
    }
}
