using FluentValidation;

namespace Application.UseCases.Commands
{
	public class UpdateAppointmentCommandValidator : AbstractValidator<UpdateAppointmentCommand>
	{
		public UpdateAppointmentCommandValidator()
		{
			RuleFor(x => x.AppointmentId)
				.NotEmpty().WithMessage("AppointmentId is required.");

			RuleFor(x => x.NewStatus)
				.IsInEnum().WithMessage("Invalid appointment status.");
		}
	}
}
