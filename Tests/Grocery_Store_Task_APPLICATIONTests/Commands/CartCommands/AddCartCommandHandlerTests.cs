using AutoMapper;
using Grocery_Store_Task_CORE.ServicesAbstraction.IProductServices;
using Grocery_Store_Task_DOMAIN.Models;
using Grocery_Store_Task_DOMAIN.RepositoriesAbstractions;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace Grocery_Store_Task_CORE.Commands.CartCommands.Tests
{

    public class AddCartCommandHandlerTests
    {
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly Mock<ITimeSlotRepository> _mockTimeSlotRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IGetRangeofProductByIdService> _mockGetRangeofProductByIdService;
        private readonly AddCartCommandHandler _handler;

        public AddCartCommandHandlerTests()
        {
            _mockCartRepository = new Mock<ICartRepository>();
            _mockTimeSlotRepository = new Mock<ITimeSlotRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockGetRangeofProductByIdService = new Mock<IGetRangeofProductByIdService>();

            _handler = new AddCartCommandHandler(
                _mockCartRepository.Object,
                _mockTimeSlotRepository.Object,
                _mockMapper.Object,
                _mockGetRangeofProductByIdService.Object
            );
        }

        //  Successful Cart Creation with Products and Existing TimeSlot
        [Fact]
        public async Task Handle_SuccessfulCartCreation_WithProductsAndExistingTimeSlot()
        {
            // Arrange
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var timeSlotDate = DateTime.Today;

            var addCartCommand = new AddCartCommand
            {
                CartProductsIds = new List<Guid> { productId1, productId2 },
                StartDate = timeSlotDate
                 ,
                IsGreen = true
            };

            var mappedCart = new Cart
            {
                Id = Guid.Empty,
                TimeSlot = new TimeSlot { StartDate = timeSlotDate, IsGreen = true }
            };

            var products = new List<Product>
        {
            new Product { Id = productId1, Name = "Apple" },
            new Product { Id = productId2, Name = "Banana" }
        };

            var existingTimeSlot = new TimeSlot
            {
                StartDate = timeSlotDate,
                IsGreen = false
            };

            _mockMapper.Setup(m => m.Map<Cart>(addCartCommand)).Returns(mappedCart);
            _mockGetRangeofProductByIdService.Setup(s => s.GetRangeofProductByIdAsync(addCartCommand.CartProductsIds))
                .ReturnsAsync(products);
            _mockTimeSlotRepository.Setup(r => r.GetTimeSlotByDateAsync(timeSlotDate))
                .ReturnsAsync(existingTimeSlot);
            _mockCartRepository.Setup(r => r.AddCart(It.IsAny<Cart>())).Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(addCartCommand, CancellationToken.None);

            // Assert
            _mockMapper.Verify(m => m.Map<Cart>(addCartCommand), Times.Once);
            _mockGetRangeofProductByIdService.Verify(s => s.GetRangeofProductByIdAsync(addCartCommand.CartProductsIds), Times.Once);
            _mockTimeSlotRepository.Verify(r => r.GetTimeSlotByDateAsync(timeSlotDate), Times.Once);
            _mockCartRepository.Verify(r => r.AddCart(It.Is<Cart>(c =>
                c.Id != Guid.Empty &&
                c.CartProducts == products &&
                c.TimeSlot.StartDate == timeSlotDate &&
                c.TimeSlot.IsGreen == addCartCommand.IsGreen
            )), Times.Once);
        }



        // Exception during Product Retrieval
        [Fact]
        public async Task Handle_ThrowsException_OnProductRetrievalFailure()
        {
            // Arrange
            var addCartCommand = new AddCartCommand
            {
                CartProductsIds = new List<Guid> { Guid.NewGuid() },
                StartDate = DateTime.Today,
                IsGreen = true
            };

            var mappedCart = new Cart();

            _mockMapper.Setup(m => m.Map<Cart>(addCartCommand)).Returns(mappedCart);
            _mockGetRangeofProductByIdService.Setup(s => s.GetRangeofProductByIdAsync(It.IsAny<List<Guid>>()))
                .ThrowsAsync(new InvalidOperationException("Database error during product retrieval"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(addCartCommand, CancellationToken.None));

            Assert.Contains("Error Handling Add Cart Command", exception.Message);
            Assert.IsType<InvalidOperationException>(exception.InnerException);

            _mockMapper.Verify(m => m.Map<Cart>(addCartCommand), Times.Once);
            _mockGetRangeofProductByIdService.Verify(s => s.GetRangeofProductByIdAsync(It.IsAny<List<Guid>>()), Times.Once);
            _mockTimeSlotRepository.Verify(r => r.GetTimeSlotByDateAsync(It.IsAny<DateTime>()), Times.Never);
            _mockCartRepository.Verify(r => r.AddCart(It.IsAny<Cart>()), Times.Never);
        }

        // Exception during TimeSlot Retrieval
        [Fact]
        public async Task Handle_ThrowsException_OnTimeSlotRetrievalFailure()
        {
            // Arrange
            var addCartCommand = new AddCartCommand
            {
                CartProductsIds = new List<Guid> { Guid.NewGuid() },
                StartDate = DateTime.Today,
                IsGreen = true
            };

            var mappedCart = new Cart();
            var products = new List<Product>();

            _mockMapper.Setup(m => m.Map<Cart>(addCartCommand)).Returns(mappedCart);
            _mockGetRangeofProductByIdService.Setup(s => s.GetRangeofProductByIdAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(products);
            _mockTimeSlotRepository.Setup(r => r.GetTimeSlotByDateAsync(It.IsAny<DateTime>()))
                .ThrowsAsync(new ApplicationException("Network error during time slot retrieval"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(addCartCommand, CancellationToken.None));

            Assert.Contains("Error Handling Add Cart Command", exception.Message);
            Assert.IsType<ApplicationException>(exception.InnerException);

            _mockMapper.Verify(m => m.Map<Cart>(addCartCommand), Times.Once);
            _mockGetRangeofProductByIdService.Verify(s => s.GetRangeofProductByIdAsync(It.IsAny<List<Guid>>()), Times.Once);
            _mockTimeSlotRepository.Verify(r => r.GetTimeSlotByDateAsync(It.IsAny<DateTime>()), Times.Once);
            _mockCartRepository.Verify(r => r.AddCart(It.IsAny<Cart>()), Times.Never);
        }

        // Exception during AddCart operation
        [Fact]
        public async Task Handle_ThrowsException_OnAddCartFailure()
        {
            // Arrange
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var timeSlotDate = DateTime.Today;

            var addCartCommand = new AddCartCommand
            {
                CartProductsIds = new List<Guid> { productId1, productId2 },
                StartDate = timeSlotDate,
                IsGreen = true
            };

            var mappedCart = new Cart
            {
                Id = Guid.Empty,
                TimeSlot = new TimeSlot { StartDate = timeSlotDate, IsGreen = true }
            };

            var products = new List<Product>
        {
            new Product { Id = productId1, Name = "Apple" },
            new Product { Id = productId2, Name = "Banana" }
        };

            var existingTimeSlot = new TimeSlot
            {
                StartDate = timeSlotDate,
                IsGreen = false
            };

            _mockMapper.Setup(m => m.Map<Cart>(addCartCommand)).Returns(mappedCart);
            _mockGetRangeofProductByIdService.Setup(s => s.GetRangeofProductByIdAsync(addCartCommand.CartProductsIds))
                .ReturnsAsync(products);
            _mockTimeSlotRepository.Setup(r => r.GetTimeSlotByDateAsync(timeSlotDate))
                .ReturnsAsync(existingTimeSlot);
            _mockCartRepository.Setup(r => r.AddCart(It.IsAny<Cart>()))
                        .ThrowsAsync(new TimeoutException("Database connection timed out during cart add"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(addCartCommand, CancellationToken.None));

            Assert.Contains("Error Handling Add Cart Command", exception.Message);
            Assert.IsType<TimeoutException>(exception.InnerException);

            _mockMapper.Verify(m => m.Map<Cart>(addCartCommand), Times.Once);
            _mockGetRangeofProductByIdService.Verify(s => s.GetRangeofProductByIdAsync(It.IsAny<List<Guid>>()), Times.Once);
            _mockTimeSlotRepository.Verify(r => r.GetTimeSlotByDateAsync(It.IsAny<DateTime>()), Times.Once);
            _mockCartRepository.Verify(r => r.AddCart(It.IsAny<Cart>()), Times.Once);
        }




    }

}