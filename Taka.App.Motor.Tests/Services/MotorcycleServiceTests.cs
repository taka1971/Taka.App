using Xunit;
using NSubstitute;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentAssertions;
using MediatR;
using Taka.App.Motor.Application.Services;
using Taka.App.Motor.Domain.Interfaces;
using Taka.App.Motor.Domain.Request;
using Taka.App.Motor.Application.Mappers;
using Taka.App.Motor.Domain.Dtos;
using Taka.App.Motor.Domain.Exceptions;
using Taka.App.Motor.Domain.Entitites;
using Taka.App.Motor.Domain.Responses;
using Taka.App.Motor.Domain.Commands;
using Taka.App.Motor.Domain.Enums;

namespace Taka.App.Motor.Tests.Services
{

    public class MotorcycleServiceTests
    {
        private readonly MotorcycleService _service;
        private readonly IMotorcycleRepository _motorcycleRepository = Substitute.For<IMotorcycleRepository>();        
        private readonly IMediator _mediator = Substitute.For<IMediator>();

        public MotorcycleServiceTests()
        {
            _service = new MotorcycleService(_motorcycleRepository, _mediator);
        }

        [Fact]
        public async Task AddAsync_ShouldAddMotorcycle()
        {
            // Arrange
            var request = new MotorcycleCreateRequest(2021, "Yamaha", "ABC-1234");            

            // Act
            var result = await _service.AddAsync(request);

            // Assert
            result.Should().BeOfType<MotorcycleResponse>();
            result.Year.Should().Be(request.Year);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotDelete_WhenRentalIsNotPermited()
        {
            // Arrange
            var rentalResponse = new RentalPermitedResponse(Guid.NewGuid(), false);

            // Act 
            var exception = await Assert.ThrowsAsync<AppException>(() => _service.DeleteAsync(rentalResponse));

            // Assert
            exception.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllMotorcycles()
        {
            // Arrange
            var motorcycles = new List<Motorcycle>
        {
            new Motorcycle { MotorcycleId = Guid.NewGuid(), Year = 2021, Model = "Model1", Plate = "XYZ-1234" },
            new Motorcycle { MotorcycleId = Guid.NewGuid(), Year = 2022, Model = "Model2", Plate = "XYZ-5678" }
        };
            _motorcycleRepository.GetAllAsync().Returns(motorcycles);

            // Act
            var results = await _service.GetAllAsync();

            // Assert
            results.Should().HaveCount(motorcycles.Count)
                   .And.ContainItemsAssignableTo<MotorcycleResponse>();
        }

        [Fact]
        public async Task RequestDeleteAsync_ShouldSendCheckRentalAvailabilityCommand()
        {
            // Arrange
            var id = Guid.NewGuid();
            var motorcycle = new Motorcycle { MotorcycleId = id };
            _motorcycleRepository.GetByIdAsync(id).Returns(Task.FromResult(motorcycle));  // Garantir que o repositório retorna uma motocicleta

            // Act
            await _service.RequestDeleteAsync(id);

            // Assert
            // Usando Arg.Is<Guid>(x => x == id) para assegurar que estamos passando o ID correto
            await _mediator.Received(1).Send(Arg.Is<CheckRentalAvailabilityCommand>(cmd => cmd.MotorcycleId == id), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateMotorcyclePlate()
        {
            // Arrange
            var motorcycle = new Motorcycle { MotorcycleId = Guid.NewGuid(), Plate = "OLD-PLATE" };
            var request = new MotorcycleUpdateRequest(motorcycle.MotorcycleId, "NEW-PLATE");
            _motorcycleRepository.GetByIdAsync(motorcycle.MotorcycleId).Returns(motorcycle);

            // Act
            await _service.UpdateAsync(request);

            // Assert
            motorcycle.Plate.Should().Be("NEW-PLATE");
            await _motorcycleRepository.Received(1).UpdateAsync(motorcycle);
        }
    }

}
