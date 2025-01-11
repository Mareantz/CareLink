using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries
{
	public class GetAppointmentsByDoctorAndDateQuery : IRequest<Result<List<AppointmentResponseDto>>>
	{
		public Guid DoctorId { get; set; }
		public DateOnly Date { get; set; }
	}
}
