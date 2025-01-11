using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PredictiveHealthcare.Infrastructure.Persistence;

namespace Application.UseCases.Commands
{
	public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
	{
		private readonly ApplicationDbContext _context;

		public CreateAppointmentCommandValidator(ApplicationDbContext context)
		{
			_context = context;

			RuleFor(x => x.Date)
				.GreaterThan(DateTime.Now)
				.WithMessage("Appointment date must be in the future.")
				.Must(BeWithinWorkingHours)
				.WithMessage("Appointment must be scheduled between 08:00 and 17:50.");

			RuleFor(x => x.Reason)
				.NotEmpty()
				.WithMessage("Reason is required.")
				.MaximumLength(500)
				.WithMessage("Reason cannot exceed 500 characters.");

			RuleFor(x => x.DoctorId)
				.NotEmpty()
				.WithMessage("Doctor ID is required.")
				.MustAsync(DoctorExists)
				.WithMessage("Doctor does not exist.");

			RuleFor(x => x)
				.MustAsync(NoDoctorOverlappingAppointments)
				.WithMessage("The doctor already has an appointment at this time.");
		}

		private static bool BeWithinWorkingHours(DateTime appointmentDate)
		{
			// Working hours are from 08:00 to 17:50
			var startTime = new TimeSpan(6, 0, 0);
			var endTime = new TimeSpan(15, 30, 0);

			var appointmentTime = appointmentDate.TimeOfDay;

			return appointmentTime >= startTime && appointmentTime <= endTime;
		}

		private async Task<bool> DoctorExists(Guid doctorId, CancellationToken cancellationToken)
		{
			return await _context.Doctors
				.AnyAsync(d => d.UserId == doctorId, cancellationToken);
		}

		private async Task<bool> NoDoctorOverlappingAppointments(CreateAppointmentCommand command, CancellationToken cancellationToken)
		{
			return !await _context.Appointments.AnyAsync(a =>
				a.DoctorId == command.DoctorId &&
				a.AppointmentDate == command.Date, cancellationToken);
		}
	}
}
