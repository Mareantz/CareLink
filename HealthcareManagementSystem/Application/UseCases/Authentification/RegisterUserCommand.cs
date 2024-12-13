using Domain.Enums;
using MediatR;
public class RegisterUserCommand : IRequest<Guid>
    {
    public string Username { get; set; }
    public string Password { get; set; }
	public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }

	// Shared Fields
	public string FirstName { get; set; }
	public string LastName { get; set; }

	// Patient-Specific Fields
	public DateOnly? DateOfBirth { get; set; }
	public string? Gender { get; set; }
	public string? Address { get; set; }

	// Doctor-Specific Fields
	public string? Specialization { get; set; }

}

