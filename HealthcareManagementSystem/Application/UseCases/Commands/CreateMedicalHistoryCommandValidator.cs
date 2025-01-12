using FluentValidation;
using System.Globalization;

namespace Application.Commands
{
    public class CreateMedicalHistoryCommandValidator : AbstractValidator<CreateMedicalHistoryCommand>
    {
        public CreateMedicalHistoryCommandValidator()
        {
            RuleFor(b => b.PatientId).NotEmpty();
            RuleFor(b => b.Diagnosis).NotEmpty().MaximumLength(100);
            RuleFor(b => b.Medication).NotEmpty().MaximumLength(100);
            RuleFor(b => b.Date).NotEmpty().WithMessage("Date must be in the format dd-MM-yyyy.");
            RuleFor(b => b.Notes).NotEmpty();
        }

    }
}
