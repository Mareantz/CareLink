using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries
{
	public class GetAppointmentByIdQuery : IRequest<Result<AppointmentResponseDto>>
	{
		public Guid Id { get; set; }
	}
}
