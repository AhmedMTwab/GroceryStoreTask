namespace Grocery_Store_Task_DOMAIN.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message: message)
        {

        }
        public NotFoundException(string resourceType, string resourceIdentifier) : base(message: $"{resourceType} with id: {resourceIdentifier} doesn't exist")
        {

        }
        public NotFoundException(string resourceType, string resourceIdentifier, string message) : base(message: message)
        {

        }
    }
}
