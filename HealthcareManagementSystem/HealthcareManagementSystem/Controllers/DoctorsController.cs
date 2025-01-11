using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application;
using Application.Commands;
using Application.Queries;
using Application.DTOs;
using Domain.Common;
using Application.UseCases.Commands;
using Application.UseCases.Queries;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator mediator;

        public DoctorsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<DoctorDto>>> GetDoctors()
        {
            return await mediator.Send(new GetDoctorsQuery());
        }

        [HttpGet("{id}")]
		public async Task<ActionResult<DoctorDto>> GetDoctor(Guid id)
		{
			return await mediator.Send(new GetDoctorByIdQuery { Id = id });
		}

		[HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateDoctor(CreateDoctorCommand command)
        {
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return StatusCode(201, result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateDoctorCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            await mediator.Send(command);
            return StatusCode(StatusCodes.Status204NoContent);
        }

    }
}
