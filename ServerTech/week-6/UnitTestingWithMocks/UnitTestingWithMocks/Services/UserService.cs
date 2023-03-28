using UnitTestingWithMocks.Interfaces;
using UnitTestingWithMocks.Models;

namespace UnitTestingWithMocks.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }
    }
}
