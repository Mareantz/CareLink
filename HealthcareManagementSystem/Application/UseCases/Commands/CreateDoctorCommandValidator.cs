using FluentValidation;

namespace Application.UseCases.Commands
{
    public class CreateDoctorCommandValidator : AbstractValidator<CreateDoctorCommand>
    {
        public CreateDoctorCommandValidator()
        {
            RuleFor(b => b.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(b => b.LastName).NotEmpty().MaximumLength(100);
			RuleFor(b => b.Specialization).NotEmpty().MaximumLength(50);
			RuleFor(b => b.Bio).NotEmpty().MaximumLength(500);
		}
    }
}
