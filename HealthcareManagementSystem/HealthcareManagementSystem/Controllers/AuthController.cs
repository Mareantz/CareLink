using Application.UseCases.Authentification;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{

	[ApiController]
	[Route("api/[controller]")]

	public class AuthController : ControllerBase
	{
		private readonly IMediator mediator;
		public AuthController(IMediator mediator)
		{
			this.mediator = mediator;
		}
		[HttpPost("register")]
		public async Task<ActionResult<Result<Guid>>> Register(RegisterUserCommand command)
		{
			var result = await mediator.Send(command);

			if (result.IsSuccess)
			{
				return StatusCode(201, result);
			}

			return BadRequest(result);
		}
		[HttpPost("login")]
		public async Task<ActionResult<Result<string>>> Login(LoginUserCommand command)
		{
			var result = await mediator.Send(command);

			if (result.IsSuccess)
			{
				return Ok(result);
			}

			return Unauthorized(result);
		}
	}
}
