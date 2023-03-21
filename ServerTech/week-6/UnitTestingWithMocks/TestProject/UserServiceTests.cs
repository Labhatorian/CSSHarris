using Moq;
using UnitTestingWithMocks.Interfaces;
using UnitTestingWithMocks.Models;
using UnitTestingWithMocks.Services;

namespace TestProject
{
    public class UserServiceTests
    {
        [Fact]
        public void GetUsers_ShouldReturnAllUsers()
        {
            // Arrange
            // Er moet getest worden of de methode GetUsers in de klasse UserService de
            // (juiste) gebruikers returned. Deze heeft echter een afhankelijkheid: IUserRepository
            // Mock IUserRepository, zodat de test uitgevoerd kan worden.
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetUsers()).Returns(new List<User>
            {
                new User { Id = 1, Name = "John Doe"},
                new User { Id = 2, Name = "Jane Doe"},
            });

            var userService = new UserService(mockUserRepository.Object);

            // Act
            var result = userService.GetUsers();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, u => u.Name == "John Doe");
            Assert.Contains(result, u => u.Name == "Jane Doe");
        }
    }
}