using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Result<Guid>> AddUser(User user);
        Task<IEnumerable<User>> GetUsers();
        Task UpdateUser(User user);
        Task<Guid> Register(User user, CancellationToken cancellationToken);
        Task<string> Login(User user);
    }
}
