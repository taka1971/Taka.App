using Microsoft.EntityFrameworkCore;
using Taka.App.Motor.Domain.Exceptions;
using Taka.App.Motor.Infra.Data.Context;
using Taka.App.Motor.Infra.Data.Repository;
using Taka.App.Motor.Domain.Entitites;
using Taka.App.Motor.Domain.Enums;

namespace Taka.App.Motor.Tests.Repositories
{
    public class MotorcycleRepositoryTests
    {
        private readonly MotorcycleRepository _repository;
        private readonly AppDbContext _dbContext;
        private static readonly DbContextOptions<AppDbContext> _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MotorcycleTestDb")
            .Options;

        public MotorcycleRepositoryTests()
        {
            _dbContext = new AppDbContext(_options);
            _repository = new MotorcycleRepository(_dbContext);
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_IfPlateExists()
        {
            // Arrange
            var motorcycle = new Motorcycle { MotorcycleId = Guid.NewGuid(), Year = 2021, Model = "Model X", Plate = "ABC-1234" };
            await _dbContext.Motorcycles.AddAsync(motorcycle);
            await _dbContext.SaveChangesAsync();

            var newMotorcycle = new Motorcycle { MotorcycleId = Guid.NewGuid(), Year = 2022, Model = "Model Y", Plate = "ABC-1234" };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => _repository.AddAsync(newMotorcycle));
            Assert.Equal(DomainErrorCode.MotorcycleAlreadyExists, exception.ErrorCode);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveMotorcycle()
        {
            // Arrange
            var motorcycle = new Motorcycle { MotorcycleId = Guid.NewGuid(), Year = 2021, Model = "Model Z", Plate = "XYZ-7890" };
            await _dbContext.Motorcycles.AddAsync(motorcycle);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(motorcycle);

            // Assert
            Assert.DoesNotContain(motorcycle, _dbContext.Motorcycles);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllMotorcycles()
        {
            // Arrange
            var motorcycle1 = new Motorcycle { MotorcycleId = Guid.NewGuid(), Year = 2021, Model = "Model A", Plate = "DEF-4567" };
            var motorcycle2 = new Motorcycle { MotorcycleId = Guid.NewGuid(), Year = 2022, Model = "Model B", Plate = "GHI-8912" };
            await _dbContext.Motorcycles.AddRangeAsync(motorcycle1, motorcycle2);
            await _dbContext.SaveChangesAsync();

            // Act
            var results = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, results.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateMotorcycleDetails()
        {
            // Arrange
            var motorcycle = new Motorcycle { MotorcycleId = Guid.NewGuid(), Year = 2021, Model = "Model C", Plate = "JKL-3456" };
            await _dbContext.Motorcycles.AddAsync(motorcycle);
            await _dbContext.SaveChangesAsync();

            // Act
            motorcycle.Plate = "MNO-7890";
            await _repository.UpdateAsync(motorcycle);
            await _dbContext.SaveChangesAsync();

            // Assert
            var updatedMotorcycle = await _dbContext.Motorcycles.FindAsync(motorcycle.MotorcycleId);
            Assert.Equal("MNO-7890", updatedMotorcycle.Plate);
        }
    }

}
