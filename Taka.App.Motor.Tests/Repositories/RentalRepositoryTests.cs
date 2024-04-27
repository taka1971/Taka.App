using NSubstitute;
using Taka.App.Motor.Domain.Exceptions;
using Refit;
using Taka.App.Motor.Infra.Data.Repository;
using Taka.App.Motor.Application.Services.Apis;
using NSubstitute.ExceptionExtensions; // Assume Refit is being used for IRentalsApiService

namespace Taka.App.Motor.Tests.Repositories
{
    public class RentalRepositoryTests
    {
        private readonly RentalRepository _repository;
        private readonly IRentalsApiService _rentalsApiService = Substitute.For<IRentalsApiService>();

        public RentalRepositoryTests()
        {
            _repository = new RentalRepository(_rentalsApiService);
        }

        [Fact]
        public async Task GetActiveRental_ShouldReturnTrue_WhenRentalIsActive()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            _rentalsApiService.ExistRentalActive(motorcycleId).Returns(Task.FromResult(true));

            // Act
            var result = await _repository.GetActiveRental(motorcycleId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetActiveRental_ShouldThrowAppException_WhenApiThrowsNotFound()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var notFoundException = ApiException.Create(null, null, new HttpResponseMessage(System.Net.HttpStatusCode.NotFound), null).Result;
            _rentalsApiService.ExistRentalActive(motorcycleId).Throws(notFoundException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AppException>(() => _repository.GetActiveRental(motorcycleId));
            Assert.Contains("Rental not found", exception.Message);
        }

        [Fact]
        public async Task GetActiveRental_ShouldThrowAppException_WhenApiThrowsBadRequest()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var badRequestException = ApiException.Create(null, null, new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest), null).Result;
            
            _rentalsApiService.ExistRentalActive(motorcycleId).Throws(badRequestException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AppException>(() => _repository.GetActiveRental(motorcycleId));
            Assert.Contains("Bad request", exception.Message);
        }
    }

}
