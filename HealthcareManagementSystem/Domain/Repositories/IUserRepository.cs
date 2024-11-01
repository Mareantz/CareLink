using Domain.Entities;

namespace Domain.Repositories
{
	public interface IUserRepository
	{
		Task<Guid> AddUser(User user);
		Task<IEnumerable<User>> GetUsers();
		Task UpdateUser(User user);
	}
}
