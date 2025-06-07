namespace Grocery_Store_Task_CORE.DTOs.DeliveryDTOs
{
    public class TimeSlotDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get { return StartDate.AddHours(1); } }
        public bool IsGreen { get; set; }
    }
}
