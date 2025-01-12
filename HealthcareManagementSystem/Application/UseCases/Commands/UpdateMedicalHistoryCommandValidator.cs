using FluentValidation;
using Application.UseCases.Commands;
using Application.DTOs;
using System;
using Application.Commands;
namespace Application.UseCases.Commands
{
    public class UpdateMedicalHistoryCommandValidator : AbstractValidator<UpdateMedicalHistoryCommand>
    {
        public UpdateMedicalHistoryCommandValidator()
        {
            RuleFor(b => b.Id).NotEmpty().Must(BeAValidGuid).WithMessage("Please specify a valid Id");
            RuleFor(b => b.Date).NotEmpty();
            RuleFor(b => b.PatientId).NotEmpty().Must(BeAValidGuid).WithMessage("Please specify a valid PatientId");
            RuleFor(b => b.Diagnosis).NotEmpty().MaximumLength(100);
            RuleFor(b => b.Medication).NotEmpty().MaximumLength(100);
            RuleFor(b => b.Notes).NotEmpty().MaximumLength(500);
        }

        private static bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
