using Domain.Enums;

namespace Domain.Entities
{
    public class User
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public UserRole Role { get; set; }
	}
}
