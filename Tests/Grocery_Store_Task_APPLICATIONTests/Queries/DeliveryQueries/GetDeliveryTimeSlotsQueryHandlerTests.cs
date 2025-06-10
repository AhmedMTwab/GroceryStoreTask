using AutoMapper;
using FluentAssertions;
using Grocery_Store_Task_CORE.DTOs.DeliveryDTOs;
using Grocery_Store_Task_CORE.Services.DeliveryServices;
using Grocery_Store_Task_CORE.ServicesAbstraction.IDeliveryServices;
using Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices;
using Grocery_Store_Task_CORE.ServicesAbstraction.ITimeSlotServices;
using Grocery_Store_Task_DOMAIN.Enums;
using Grocery_Store_Task_DOMAIN.Models;
using Moq;
using Xunit;

namespace Grocery_Store_Task_CORE.Queries.DeliveryQueries.Tests
{
    public class GetDeliveryTimeSlotsQueryHandlerTests
    {
        private readonly Mock<IGetRangeofProductByIdService> _mockProductsByIdServices;
        private readonly Mock<IGetMaximumDeliveryTypeService> _mockGetMaximumDeliveryType;
        private readonly Mock<IGetDeliveryStartDateService> _mockGetDeliveryStartDate;
        private readonly Mock<IGenerateTimeSlotsService> _mockGenerateTimeSlots;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetDeliveryTimeSlotsQueryHandler _handler;

        public GetDeliveryTimeSlotsQueryHandlerTests()
        {
            _mockProductsByIdServices = new Mock<IGetRangeofProductByIdService>();
            _mockGetMaximumDeliveryType = new Mock<IGetMaximumDeliveryTypeService>();
            _mockGetDeliveryStartDate = new Mock<IGetDeliveryStartDateService>();
            _mockGenerateTimeSlots = new Mock<IGenerateTimeSlotsService>();
            _mockMapper = new Mock<IMapper>();


            _handler = new GetDeliveryTimeSlotsQueryHandler(
                _mockProductsByIdServices.Object,
                _mockGetMaximumDeliveryType.Object,
                _mockGetDeliveryStartDate.Object,
                _mockGenerateTimeSlots.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task Handle_ReturnsTimeSlotDTOs_OnSuccess()
        {
            // Arrange 
            var orderDate = new DateTime(2025, 6, 7, 10, 0, 0);
            var product1Id = Guid.NewGuid();
            var product2Id = Guid.NewGuid();
            var query = new GetDeliveryTimeSlotsQuery
            {
                productIds = new List<Guid> { product1Id, product2Id },
                orderDate = orderDate
            };

            var products = new List<Product>
        {
            new Product { Id = product1Id, Name = "Laptop", Category = ProductTypeEnum.InStock },
            new Product { Id = product2Id, Name = "Milk", Category = ProductTypeEnum.ExternalProduct }
        };
            var minimumDeliveryType = ProductTypeEnum.ExternalProduct;
            var firstTimeSlotDate = new DateTime(2025, 6, 8, 9, 0, 0);
            var generatedTimeSlots = new List<TimeSlot>
        {
            new TimeSlot { StartDate = new DateTime(2025, 6, 8, 9, 0, 0) , IsGreen = true },
            new TimeSlot { StartDate = new DateTime(2025, 6, 8, 10, 0, 0), IsGreen = false }
        };
            var expectedTimeSlotDTOs = new List<TimeSlotDTO>
        {
            new TimeSlotDTO { StartDate = new DateTime(2025, 6, 8, 9, 0, 0), IsGreen = true },
            new TimeSlotDTO { StartDate = new DateTime(2025, 6, 8, 10, 0, 0), IsGreen = false }
        };

            _mockProductsByIdServices.Setup(s => s.GetRangeofProductByIdAsync(query.productIds))
                .ReturnsAsync(products);
            _mockGetMaximumDeliveryType.Setup(s => s.GetOrderMaximumDeliveryType(products))
                .Returns(minimumDeliveryType);
            _mockGetDeliveryStartDate.Setup(s => s.GetStartDate(minimumDeliveryType, query.orderDate))
                .Returns(firstTimeSlotDate);
            _mockGenerateTimeSlots.Setup(s => s.GenerateSlotsFromStartDate(firstTimeSlotDate, query.orderDate, 14, minimumDeliveryType))
                .ReturnsAsync(generatedTimeSlots);
            _mockMapper.Setup(m => m.Map<IEnumerable<TimeSlotDTO>>(generatedTimeSlots))
                .Returns(expectedTimeSlotDTOs);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedTimeSlotDTOs);

            _mockProductsByIdServices.Verify(s => s.GetRangeofProductByIdAsync(query.productIds), Times.Once);
            _mockGetMaximumDeliveryType.Verify(s => s.GetOrderMaximumDeliveryType(products), Times.Once);
            _mockGetDeliveryStartDate.Verify(s => s.GetStartDate(minimumDeliveryType, query.orderDate), Times.Once);
            _mockGenerateTimeSlots.Verify(s => s.GenerateSlotsFromStartDate(firstTimeSlotDate, query.orderDate, 14, minimumDeliveryType), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<TimeSlotDTO>>(generatedTimeSlots), Times.Once);
        }



        [Fact]
        public async Task Handle_ThrowsGenericException_OnServiceFailure()
        {
            // Arrange
            var orderDate = new DateTime(2025, 6, 7, 15, 0, 0);
            var query = new GetDeliveryTimeSlotsQuery
            {
                productIds = new List<Guid> { Guid.NewGuid() },
                orderDate = orderDate
            };

            _mockProductsByIdServices.Setup(s => s.GetRangeofProductByIdAsync(It.IsAny<List<Guid>>()))
                .ThrowsAsync(new InvalidOperationException("Simulated database error"));

            // Act
            Func<Task> action = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<Exception>()
                .WithMessage("Error Fetching Time Slots");

            _mockProductsByIdServices.Verify(s => s.GetRangeofProductByIdAsync(query.productIds), Times.Once);
            _mockGetMaximumDeliveryType.Verify(s => s.GetOrderMaximumDeliveryType(It.IsAny<IEnumerable<Product>>()), Times.Never);
            _mockGetDeliveryStartDate.Verify(s => s.GetStartDate(It.IsAny<ProductTypeEnum>(), It.IsAny<DateTime>()), Times.Never);
            _mockGenerateTimeSlots.Verify(s => s.GenerateSlotsFromStartDate(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<ProductTypeEnum>()), Times.Never);
            _mockMapper.Verify(m => m.Map<IEnumerable<TimeSlotDTO>>(It.IsAny<IEnumerable<TimeSlot>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ThrowsGenericException_IfGetOrderMaximumDeliveryTypeThrows()
        {
            // Arrange
            var orderDate = new DateTime(2025, 6, 7, 18, 0, 0);
            var query = new GetDeliveryTimeSlotsQuery
            {
                productIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
                orderDate = orderDate
            };
            var products = new List<Product> { new Product { Id = Guid.NewGuid(), Name = "Item", Category = ProductTypeEnum.FreshFood } };

            _mockProductsByIdServices.Setup(s => s.GetRangeofProductByIdAsync(query.productIds))
                .ReturnsAsync(products);

            _mockGetMaximumDeliveryType.Setup(s => s.GetOrderMaximumDeliveryType(products))
                .Throws(new ArgumentException("Invalid product list for delivery type calculation"));

            // Act
            Func<Task> action = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<Exception>()
                .WithMessage("Error Fetching Time Slots");


            _mockProductsByIdServices.Verify(s => s.GetRangeofProductByIdAsync(query.productIds), Times.Once);
            _mockGetMaximumDeliveryType.Verify(s => s.GetOrderMaximumDeliveryType(products), Times.Once);
            _mockGetDeliveryStartDate.Verify(s => s.GetStartDate(It.IsAny<ProductTypeEnum>(), It.IsAny<DateTime>()), Times.Never);
            _mockGenerateTimeSlots.Verify(s => s.GenerateSlotsFromStartDate(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<ProductTypeEnum>()), Times.Never);
            _mockMapper.Verify(m => m.Map<IEnumerable<TimeSlotDTO>>(It.IsAny<IEnumerable<TimeSlot>>()), Times.Never);
        }
    }
}