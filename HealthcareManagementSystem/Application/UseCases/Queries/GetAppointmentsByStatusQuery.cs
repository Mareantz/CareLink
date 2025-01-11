using Application.DTOs;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Queries
{
	public class GetAppointmentsByStatusQuery : IRequest<Result<List<AppointmentResponseDto>>>
	{
		public AppointmentStatus Status { get; set; }
	}
}
