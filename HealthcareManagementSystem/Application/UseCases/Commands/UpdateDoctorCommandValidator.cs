using FluentValidation;

namespace Application.UseCases.Commands
{
    public class UpdateDoctorCommandValidator : AbstractValidator<UpdateDoctorCommand>
    {
        public UpdateDoctorCommandValidator()
        {
            RuleFor(b => b.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(b => b.LastName).NotEmpty().MaximumLength(100);
            RuleFor(b => b.Gender).NotEmpty();
            RuleFor(b => b.Email).NotEmpty();
            RuleFor(b => b.PhoneNumber).NotEmpty();
            RuleFor(b => b.Address).NotEmpty();
        }
    }
}
