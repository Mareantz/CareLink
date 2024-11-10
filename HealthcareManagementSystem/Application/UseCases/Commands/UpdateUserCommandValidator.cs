using FluentValidation;

namespace Application.Commands
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(b => b.Username).NotEmpty().MaximumLength(100);
            RuleFor(b => b.PasswordHash).NotEmpty().MinimumLength(10);
            RuleFor(b => b.Email).NotEmpty();
            RuleFor(b => b.PhoneNumber).NotEmpty();
        }
    }
}
