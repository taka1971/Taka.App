using MongoDB.Driver;
using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Infra.Data.Repositories;
using Moq;

namespace Taka.App.Authentications.Tests.Validations
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _userRepository;
        private readonly Mock<IMongoCollection<User>> _mockUserCollection;
        private readonly Mock<IAsyncCursor<User>> _mockUserCursor;

        public UserRepositoryTests()
        {
            var mockDatabase = new Mock<IMongoDatabase>();
            _mockUserCollection = new Mock<IMongoCollection<User>>();
            _mockUserCursor = new Mock<IAsyncCursor<User>>();

            // Configure the database setup
            mockDatabase
                .Setup(db => db.GetCollection<User>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                .Returns(_mockUserCollection.Object);

            // Create the repository instance
            _userRepository = new UserRepository(mockDatabase.Object);                        
            _mockUserCursor.Setup(_ => _.Current).Returns(new List<User>());              
            _mockUserCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                           .Returns(true)  // Simulate data presence
                           .Returns(false);  // End of data
            _mockUserCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(true)
                           .ReturnsAsync(false);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldInsertUserIntoDatabase()
        {
            // Arrange
            var user = new User { Email = "test@example.com", PasswordHash = "password" };

            // Act
            await _userRepository.CreateUserAsync(user);

            // Assert
            _mockUserCollection.Verify(c => c.InsertOneAsync(user, null, default(CancellationToken)), Times.Once);
        }

    }
}

