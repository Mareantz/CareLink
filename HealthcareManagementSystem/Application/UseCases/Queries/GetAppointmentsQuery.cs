using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries
{
	public class GetAppointmentsQuery : IRequest<Result<List<AppointmentResponseDto>>>
	{
	}
}
