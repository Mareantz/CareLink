using Application.UseCases.Commands;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class AppointmentsController : ControllerBase
	{
		private readonly IMediator mediator;

		public AppointmentsController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpPost]
		public async Task<ActionResult<Result<Guid>>> CreateAppointment(CreateAppointmentCommand command)
		{
			var result = await mediator.Send(command);
			if (result.IsSuccess)
			{
				return StatusCode(201, result.Data);
			}
			return BadRequest(result.ErrorMessage);
		}
	}
}
