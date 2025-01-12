using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Commands
{
	public class UpdateAppointmentCommand : IRequest<Result>
	{
		public Guid AppointmentId { get; set; }
		public AppointmentStatus NewStatus { get; set; }
	}
}