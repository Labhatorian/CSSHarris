using UnitTestingWithMocks.Models;

namespace UnitTestingWithMocks.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
    }
}
