using Application.UseCases.Commands;
using Application.UseCases.Queries;
using Domain.Common;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	[Authorize]
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

		[HttpGet]
		public async Task<ActionResult<Result<List<AppointmentResponseDto>>>> GetAppointments()
		{
			var query = new GetAppointmentsQuery();
			var result = await mediator.Send(query);
			if (result.IsSuccess)
				return Ok(result);
			return BadRequest(result.ErrorMessage);
		}

		[HttpGet("date/{date}")]
		public async Task<ActionResult<Result<List<AppointmentResponseDto>>>> GetAppointmentsByDate(DateTime date)
		{
			var query = new GetAppointmentsByDateQuery { Date = date };
			var result = await mediator.Send(query);

			if (result.IsSuccess)
				return Ok(result);

			return BadRequest(result.ErrorMessage);
		}

		[HttpGet("status/{status}")]
		public async Task<ActionResult<Result<List<AppointmentResponseDto>>>> GetAppointmentsByStatus(AppointmentStatus status)
		{
			var query = new GetAppointmentsByStatusQuery { Status = status };
			var result = await mediator.Send(query);

			if (result.IsSuccess)
				return Ok(result);

			return BadRequest(result.ErrorMessage);
		}

		[HttpGet("doctor/{doctorId}")]
		[Authorize(Roles = "Doctor")]
		public async Task<ActionResult<Result<List<AppointmentResponseDto>>>> GetAppointmentsByDoctorId(Guid doctorId)
		{
			var query = new GetAppointmentsByDoctorIdQuery { DoctorId = doctorId };
			var result = await mediator.Send(query);

			if (result.IsSuccess)
				return Ok(result);

			return BadRequest(result.ErrorMessage);
		}

		[HttpGet("patient/{patientId}")]
		[Authorize(Roles = "Patient")]
		public async Task<ActionResult<Result<List<AppointmentResponseDto>>>> GetAppointmentsByPatientId(Guid patientId)
		{
			var query = new GetAppointmentsByPatientIdQuery { PatientId = patientId };
			var result = await mediator.Send(query);

			if (result.IsSuccess)
				return Ok(result);

			return BadRequest(result.ErrorMessage);
		}
	}
}
