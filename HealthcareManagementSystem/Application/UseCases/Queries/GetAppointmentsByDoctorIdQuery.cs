using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries
{
	public class GetAppointmentsByDoctorIdQuery : IRequest<Result<List<AppointmentResponseDto>>>
	{
		public Guid DoctorId { get; set; }
	}
}
