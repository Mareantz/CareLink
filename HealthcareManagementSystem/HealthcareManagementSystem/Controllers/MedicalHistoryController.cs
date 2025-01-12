using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Commands;
using Application.Queries;
using Application.DTOs;
using Domain.Common;
using Application.UseCases.Queries;
using Microsoft.AspNetCore.Authorization;
using Application.UseCases.Commands;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace HealthcareManagementSystem.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	[Authorize]
	public class MedicalHistoryController : ControllerBase
	{
		private readonly IMediator mediator;
		private readonly IHostEnvironment hostEnvironment;
		public MedicalHistoryController(IMediator mediator, IHostEnvironment hostEnvironment)
		{
			this.mediator = mediator;
			this.hostEnvironment = hostEnvironment;
		}
		[HttpGet]
		public async Task<ActionResult<IEnumerable<MedicalHistoryDTO>>> GetMedicalHistories()
		{
			var histories = await mediator.Send(new GetMedicalHistoriesQuery());
			return Ok(histories);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<MedicalHistoryDTO>> GetMedicalHistory(Guid id)
		{
			var history = await mediator.Send(new GetMedicalHistoryByIdQuery { Id = id });
			if (history == null)
			{
				return NotFound();
			}
			return Ok(history);
		}
		[HttpPost]
		public async Task<ActionResult<Result<Guid>>> CreateMedicalHistory([FromForm] CreateMedicalHistoryRequest request)
		{
			// Validate the request model
			if (request.Attachments != null && request.Attachments.Count > 0)
			{
				var uploadResults = await SaveFilesAsync(request.Attachments);
				if (!uploadResults.IsSuccess)
				{
					return BadRequest(uploadResults.ErrorMessage);
				}
				request.AttachmentsPaths = uploadResults.Data;
			}

			// Create the command with file paths
			var command = new CreateMedicalHistoryCommand
			{
				Diagnosis = request.Diagnosis,
				Medication = request.Medication,
				DateRecorded = request.DateRecorded,
				Notes = request.Notes,
				PatientId = request.PatientId,
				Attachments = request.AttachmentsPaths
			};

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
			var result = await mediator.Send(command);
			if (result.IsSuccess)
			{
				return NoContent();
			}
			return BadRequest(result.ErrorMessage);
		}
		[HttpGet("filtered")]
		public async Task<ActionResult<Result<PagedResult<MedicalHistoryDTO>>>> GetFilteredMedicalHistories(
			[FromQuery] int page,
			[FromQuery] int pageSize,
			[FromQuery] Guid? patientId,
			[FromQuery] Guid? id,
			[FromQuery] string? diagnosis,
			[FromQuery] string? medication,
			[FromQuery] DateTime? daterecorded)
		{
			var query = new GetFilteredMedicalHistoryQuery
			{
				Page = page,
				PageSize = pageSize,
				PatientId = patientId,
				Id = id,
				Diagnosis = diagnosis,
				Medication = medication,
				DateRecorded = daterecorded
			};

			var result = await mediator.Send(query);
			return Ok(result);
		}

		[HttpGet("patient/{patientId}")]
		public async Task<ActionResult<Result<IEnumerable<MedicalHistoryDTO>>>> GetMedicalHistoriesByPatientId(Guid patientId)
		{
			var query = new GetMedicalHistoryByPatientIdQuery { PatientId = patientId };
			var result = await mediator.Send(query);
			if (!result.IsSuccess)
			{
				return NotFound(result.ErrorMessage);
			}
			return Ok(result.Data);
		}

		private async Task<Result<List<string>>> SaveFilesAsync(List<IFormFile> files)
		{
			var filePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			try
			{
				// Define the path to save files (e.g., wwwroot/uploads/medicalHistories)
				var uploadsFolder = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot", "uploads", "medicalHistories");
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}

				foreach (var file in files)
				{
					if (file.Length > 0)
					{
						// Generate a unique file name to prevent overwriting
						var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
						var filePath = Path.Combine(uploadsFolder, uniqueFileName);

						using (var stream = new FileStream(filePath, FileMode.Create))
						{
							await file.CopyToAsync(stream);
						}

						// Store the relative path to be saved in the database
						var relativePath = Path.Combine("uploads", "medicalHistories", uniqueFileName).Replace("\\", "/");
						filePaths.Add(relativePath);
					}
				}

				return Result<List<string>>.Success(filePaths.ToList());
			}
			catch (Exception ex)
			{
				// Log the exception as needed
				return Result<List<string>>.Failure($"File upload failed: {ex.Message}");
			}
		}

	}

	public class CreateMedicalHistoryRequest
	{
		[FromForm] public string? Diagnosis { get; set; }
		[FromForm] public string? Medication { get; set; }
		[FromForm] public DateTime DateRecorded { get; set; }
		[FromForm] public string? Notes { get; set; }
		[FromForm] public Guid PatientId { get; set; }
		[FromForm] public List<IFormFile>? Attachments { get; set; }
		[BindNever] public List<string> AttachmentsPaths { get; set; } = new List<string>();
	}
}
