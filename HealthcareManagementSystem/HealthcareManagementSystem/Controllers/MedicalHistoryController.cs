using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Commands;
using Application.Queries;
using Application.DTOs;
using Domain.Common;
using Application.UseCases.Queries;
using Microsoft.AspNetCore.Authorization;
using Application.UseCases.Commands;
namespace HealthcareManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalHistoryController : ControllerBase
    {
        private readonly IMediator mediator;
        public MedicalHistoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalHistoryDTO>>> GetMedicalHistories()
        {
            return await mediator.Send(new GetMedicalHistoriesQuery());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalHistoryDTO>> GetMedicalHistory(Guid id)
        {
            return await mediator.Send(new GetMedicalHistoryByIdQuery { Id = id });
        }
        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateMedicalHistory(CreateMedicalHistoryCommand command)
        {
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return StatusCode(201, result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalHistory(Guid id, UpdateMedicalHistoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Medical History ID mismatch.");
            }
            await mediator.Send(command);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpGet("filtered")]
        [Authorize(Policy = "DoctorOnly")]
        public async Task<ActionResult<Result<PagedResult<MedicalHistoryDTO>>>> GetFilteredMedicalHistories(
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] Guid? patientId,
            [FromQuery] Guid? id,
            [FromQuery] string? diagnosis,
            [FromQuery] string? medication,
            [FromQuery] DateTime? daterecorded)
        {
            return await mediator.Send(new GetFilteredMedicalHistoryQuery
            {
                Page = page,
                PageSize = pageSize,
                PatientId = patientId,
                Id = id,
                Diagnosis = diagnosis,
                Medication = medication,
                DateRecorded = daterecorded
            });
        }
    }
}
