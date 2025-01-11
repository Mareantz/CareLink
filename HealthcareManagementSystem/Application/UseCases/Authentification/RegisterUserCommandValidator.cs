using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PredictiveHealthcare.Infrastructure.Persistence;
using Domain.Enums;

namespace Application.UseCases.Authentification
{
	public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
	{
		private readonly ApplicationDbContext _dbContext;
		public RegisterUserCommandValidator(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
			RuleFor(b => b.Username).NotEmpty().MaximumLength(50).MustAsync(BeUniqueUsername)
				.WithMessage("Username is too long or already in use.");
			RuleFor(b => b.Password).NotEmpty().MinimumLength(10);
			RuleFor(b => b.ConfirmPassword).Equal(b => b.Password)
				.WithMessage("Passwords do not match.");
			RuleFor(b => b.Email).NotEmpty().EmailAddress().MustAsync(BeUniqueEmail)
				.WithMessage("Email is already in use.");
			RuleFor(b => b.PhoneNumber).NotEmpty().Matches(@"^\d{10}$")
				.WithMessage("Phone number must be 10 digits.");
			RuleFor(b => b.FirstName).NotEmpty().MaximumLength(100);
			RuleFor(b => b.LastName).NotEmpty().MaximumLength(100);
			RuleFor(b => b.DateOfBirth)
				.NotEmpty()
				.Must(BeAValidDate)
				.WithMessage("Date of birth must be a valid date, in the past, and the user must be at least 18 years old.")
				.When(b => b.Role == UserRole.Patient);
			RuleFor(b => b.Gender).NotEmpty()
				.When(b => b.Role == UserRole.Patient);
			RuleFor(b => b.Address).NotEmpty()
				.When(b => b.Role == UserRole.Patient);
			RuleFor(b => b.Specialization).NotEmpty()
				.When(b => b.Role == UserRole.Doctor);
		}

		private static bool BeAValidDate(DateOnly? nullableDate)
		{
			if (!nullableDate.HasValue)
				return false;

			var date = nullableDate.Value;

			if (date > DateOnly.FromDateTime(DateTime.Now))
				return false;

			var eighteenYearsAgo = DateOnly.FromDateTime(DateTime.Now.AddYears(-18));
			return date <= eighteenYearsAgo;
		}

		private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
		{
			return !await _dbContext.Users.AnyAsync(u => u.Username == username, cancellationToken);
		}

		private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
		{
			return !await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
		}
	}
}
