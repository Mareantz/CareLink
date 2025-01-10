using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries
{
	public class GetFilteredPatientsQuery : IRequest<Result<PagedResult<PatientDto>>>
	{
		public int Page { get; set; }
		public int PageSize { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Gender { get; set; }
		public DateOnly? DateOfBirth { get; set; }
	}
}
