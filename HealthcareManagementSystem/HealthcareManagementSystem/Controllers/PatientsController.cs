using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application;
using Application.Commands;
using Application.Queries;
using Application.DTOs;
using Domain.Common;
using Application.UseCases.Queries;

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
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatient(Guid id)
        {
            return await mediator.Send(new GetPatientByIdQuery { Id = id });
        }
        [HttpGet]
        public async Task<ActionResult<List<PatientDto>>> GetPatients()
        {
            return await mediator.Send(new GetPatientsQuery());
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreatePatient(CreatePatientCommand command)
        {
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return StatusCode(201, result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatePatientCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Patient ID mismatch.");
            }

         await mediator.Send(command);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
