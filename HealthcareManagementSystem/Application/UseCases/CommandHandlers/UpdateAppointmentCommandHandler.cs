using Application.UseCases.Commands;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers
{
	public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, Result>
	{
		private readonly IAppointmentRepository _appointmentRepository;

		public UpdateAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
		{
			_appointmentRepository = appointmentRepository;
		}

		public async Task<Result> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
		{
			var appointment = await _appointmentRepository.GetAppointmentByIdAsync(request.AppointmentId);

			if (appointment == null)
			{
				return Result.Failure("Appointment not found.");
			}

			appointment.Status = request.NewStatus;

			var updateResult = await _appointmentRepository.UpdateAppointment(appointment);

			if (!updateResult.IsSuccess)
			{
				return Result.Failure("Failed to update the appointment status.");
			}

			return Result.Success();
		}
	}
}
