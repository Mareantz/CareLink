using FluentValidation;
using System.Globalization;

namespace Application.Commands
{
    public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientCommandValidator()
        {
            RuleFor(b => b.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(b => b.LastName).NotEmpty().MaximumLength(100);
            RuleFor(b => b.DateOfBirth).NotEmpty().Must(BeAValidDate).WithMessage("Date of birth must be in the format dd-MM-yyyy.");
            RuleFor(b => b.Gender).NotEmpty();
            RuleFor(b => b.Address).NotEmpty();
        }
        private static bool BeAValidDate(string? date)
        {
			if (string.IsNullOrEmpty(date))
				return false;
			return DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
