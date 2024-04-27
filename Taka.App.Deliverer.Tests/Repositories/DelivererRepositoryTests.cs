using Microsoft.EntityFrameworkCore;
using Taka.App.Deliverer.Domain.Entities;
using Taka.App.Deliverer.Domain.Exceptions;
using Taka.App.Deliverer.Infra.Data.Context;
using Taka.App.Deliverer.Infra.Data.Repositories;
using Taka.App.Deliverer.Domain.Enums;


namespace Taka.App.Deliverer.Tests.Repositories
{
    public class DelivererRepositoryTests
    {        
        private readonly DelivererRepository _repository;
        private readonly AppDbContext _dbContext;
        private static readonly DbContextOptions<AppDbContext> _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestInMemoryDb")
            .Options;

        public DelivererRepositoryTests()
        {
            _dbContext = new AppDbContext(_options);
            _repository = new DelivererRepository(_dbContext);
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_IfCNPJExists()
        {
            // Arrange          
            var deliverer = new PersonDelivery { Id = Guid.NewGuid(), CNPJ = "unique_cnpj", CNHNumber = "unique_cnh", Name = "Deliverer 1", CNHImageUrl = "url" };
            _dbContext.Deliverers.Add(deliverer);
            await _dbContext.SaveChangesAsync();

            var newDeliverer = new PersonDelivery { Id = Guid.NewGuid(), CNPJ = "unique_cnpj", CNHNumber = "outer_cnh", Name = "Deliverer 2", CNHImageUrl = "url" };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => _repository.AddAsync(newDeliverer));
            Assert.Equal(DomainErrorCode.DelivererAlreadyExists, exception.ErrorCode);
        }        

        [Fact]
        public async Task DeleteAsync_ShouldRemoveDeliverer()
        {
            // Arrange
            var deliverer = new PersonDelivery { Id = Guid.NewGuid(), CNPJ = "unique_cnpj", CNHNumber = "unique_cnh", Name="Deliverer", CNHImageUrl="url" };
            _dbContext.Deliverers.Add(deliverer);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(deliverer);

            // Assert
            Assert.DoesNotContain(deliverer, _dbContext.Deliverers);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllItems()
        {
            // Arrange            
            var deliverer = new PersonDelivery { Id = Guid.NewGuid(), CNPJ = "cnpj1", CNHNumber = "cnh1", Name = "Deliverer 1", CNHImageUrl = "url" };
            _dbContext.Deliverers.Add(deliverer);
            await _dbContext.SaveChangesAsync();

            deliverer = new PersonDelivery { Id = Guid.NewGuid(), CNPJ = "cnpj2", CNHNumber = "cnh2", Name = "Deliverer 1", CNHImageUrl = "url" };
            _dbContext.Deliverers.Add(deliverer);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldChangeData()
        {
            // Arrange
            var deliverer = new PersonDelivery { Id = Guid.NewGuid(), CNPJ = "original_cnpj", CNHNumber = "original_cnh", Name= "Deliverer", CNHImageUrl="url" };
            await _dbContext.Deliverers.AddAsync(deliverer);
            await _dbContext.SaveChangesAsync();

            // Act
            deliverer.CNPJ = "updated_cnpj";
            await _repository.UpdateAsync(deliverer);
            await _dbContext.SaveChangesAsync();

            // Assert
            var updatedDeliverer = await _dbContext.Deliverers.FindAsync(deliverer.Id);
            Assert.Equal("updated_cnpj", updatedDeliverer?.CNPJ);
        }
    }
}