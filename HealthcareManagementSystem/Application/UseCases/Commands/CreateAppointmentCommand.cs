using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands
{
	public class CreateAppointmentCommand : IRequest<Result<Guid>>
	{
		public required Guid PatientId { get; set; }
		public required Guid DoctorId { get; set; }
		public required DateTime Date { get; set; }
		public required string Reason { get; set; }
	}
}
