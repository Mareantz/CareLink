using Domain.Enums;
using MediatR;

namespace Application.UseCases.Authentification
{
	public class RegisterUserCommand : IRequest<Guid>
	{
		public required string Username { get; set; }
		public required string Password { get; set; }
		public required string ConfirmPassword { get; set; }
		public required string Email { get; set; }
		public required string PhoneNumber { get; set; }
		public UserRole Role { get; set; }

		// Shared Fields
		public required string FirstName { get; set; }
		public required string LastName { get; set; }

		// Patient-Specific Fields
		public DateOnly? DateOfBirth { get; set; }
		public string? Gender { get; set; }
		public string? Address { get; set; }

		// Doctor-Specific Fields
		public string? Specialization { get; set; }

	}
}

