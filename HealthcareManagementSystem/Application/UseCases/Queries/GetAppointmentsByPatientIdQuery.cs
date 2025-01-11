using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries
{
	public class GetAppointmentsByPatientIdQuery : IRequest<Result<List<AppointmentResponseDto>>>
	{
		public Guid PatientId { get; set; }
	}
}
