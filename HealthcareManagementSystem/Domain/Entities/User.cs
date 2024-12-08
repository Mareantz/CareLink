using Domain.Enums;

namespace Domain.Entities
{
    public class User
	{
		public Guid Id { get; set; }
		public required string Username { get; set; }
		public required string PasswordHash { get; set; }
		public required string? Email { get; set; }
		public required string? PhoneNumber { get; set; }
		public UserRole Role { get; set; }
	}
}
