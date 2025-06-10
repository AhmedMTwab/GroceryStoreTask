using FluentAssertions;
using Grocery_Store_Task_CORE.DTOs.ProductDTOs;
using Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices;
using Grocery_Store_Task_DOMAIN.Exceptions;
using Moq;
using Xunit;

namespace Grocery_Store_Task_CORE.Queries.ProductQueries.Tests
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IGetAllProductService> _mockGetAllProductService;
        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsQueryHandlerTests()
        {
            _mockGetAllProductService = new Mock<IGetAllProductService>();


            _handler = new GetAllProductsQueryHandler(_mockGetAllProductService.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAllProducts_OnSuccess()
        {
            // Arrange
            var query = new GetAllProductsQuery();

            var expectedProducts = new List<GetProductDTO>
        {
            new GetProductDTO { Id = Guid.NewGuid(), Name = "Apple" },
            new GetProductDTO { Id = Guid.NewGuid(), Name = "Bread" }
        };

            _mockGetAllProductService.Setup(s => s.GetAllProductsAsync())
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedProducts);
            _mockGetAllProductService.Verify(s => s.GetAllProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsNotFoundException_WhenNoProductsFound()
        {
            // Arrange
            var query = new GetAllProductsQuery();

            _mockGetAllProductService.Setup(s => s.GetAllProductsAsync())
                .ReturnsAsync((IEnumerable<GetProductDTO>)null);

            // Act
            Func<Task> action = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<Exception>()
                .WithMessage("Error Fetching Products")
                .WithInnerException<Exception, NotFoundException>();


            _mockGetAllProductService.Verify(s => s.GetAllProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenServiceReturnsEmptyList()
        {
            // Arrange
            var query = new GetAllProductsQuery();
            var emptyProducts = new List<GetProductDTO>();

            _mockGetAllProductService.Setup(s => s.GetAllProductsAsync())
                .ReturnsAsync(emptyProducts);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _mockGetAllProductService.Verify(s => s.GetAllProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsGenericException_WhenServiceThrowsException()
        {
            // Arrange
            var query = new GetAllProductsQuery();
            var innerException = new InvalidOperationException("Database connection failed.");

            _mockGetAllProductService.Setup(s => s.GetAllProductsAsync())
                .ThrowsAsync(innerException);

            // Act
            Func<Task> action = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<Exception>()
                .WithMessage("Error Fetching Products");

            _mockGetAllProductService.Verify(s => s.GetAllProductsAsync(), Times.Once);
        }
    }
}