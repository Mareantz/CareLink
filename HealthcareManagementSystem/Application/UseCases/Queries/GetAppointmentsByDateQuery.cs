using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries
{
	public class GetAppointmentsByDateQuery : IRequest<Result<List<AppointmentResponseDto>>>
	{
		public DateTime Date { get; set; }
	}
}
