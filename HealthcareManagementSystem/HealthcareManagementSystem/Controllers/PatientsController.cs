using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application;
using Application.Commands;
using Application.Queries;
using Application.DTOs;

namespace HealthcareManagementSystem.Controllers
{
	[Route("api/v1/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
	{
        private readonly IMediator mediator;

        public PatientsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<PatientDTO>>> GetPatients()
        {
            return await mediator.Send(new GetPatientsQuery());
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreatePatient(CreatePatientCommand command)
        {
            var id = await mediator.Send(command);
            return StatusCode(201, id);
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, UpdatePatientCommand command)
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
