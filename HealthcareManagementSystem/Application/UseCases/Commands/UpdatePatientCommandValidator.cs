using FluentValidation;

namespace Application.Commands
{
    public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
    {
        public UpdatePatientCommandValidator()
        {
            RuleFor(b => b.Id).NotEmpty().Must(BeAValidGuid).WithMessage("Please specify a valid Id");
            RuleFor(b => b.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(b => b.LastName).NotEmpty().MaximumLength(100);
            RuleFor(b => b.DateOfBirth).NotEmpty();
            RuleFor(b => b.Gender).NotEmpty();
            RuleFor(b => b.Address).NotEmpty();
        }
        private bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
