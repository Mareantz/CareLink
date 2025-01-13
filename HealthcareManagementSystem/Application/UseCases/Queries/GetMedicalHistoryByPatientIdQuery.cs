using MediatR;
using Application.DTOs;
using Domain.Common;

namespace Application.UseCases.Queries
{
	public class GetMedicalHistoryByPatientIdQuery : IRequest<Result<List<MedicalHistoryDTO>>>
	{
		public Guid PatientId { get; set; }

	}
}
