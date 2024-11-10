using FluentValidation;

namespace Application.UseCases.Commands
{
    public class UpdateDoctorCommandValidator : AbstractValidator<UpdateDoctorCommand>
    {
        public UpdateDoctorCommandValidator()
        {
			RuleFor(b => b.Id).NotEmpty().Must(BeAValidGuid).WithMessage("Please specify a valid Id");
			RuleFor(b => b.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(b => b.LastName).NotEmpty().MaximumLength(100);
			RuleFor(b => b.Specialization).NotEmpty().MaximumLength(50);
			RuleFor(b => b.Bio).NotEmpty().MaximumLength(500);
		}

        private static bool BeAValidGuid(Guid guid)
		{
			return Guid.TryParse(guid.ToString(), out _);
		}
	}
}
