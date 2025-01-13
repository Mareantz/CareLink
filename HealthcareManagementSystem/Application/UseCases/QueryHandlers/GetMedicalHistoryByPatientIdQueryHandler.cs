using MediatR;
using Application.DTOs;
using Domain.Common;
using Application.UseCases.Queries;
using Microsoft.EntityFrameworkCore;
using PredictiveHealthcare.Infrastructure.Persistence;
using Domain.Repositories;
using AutoMapper;

namespace Application.UseCases.QueryHandlers
{
	public class GetMedicalHistoryByPatientIdQueryHandler : IRequestHandler<GetMedicalHistoryByPatientIdQuery, Result<List<MedicalHistoryDTO>>>
	{
		private readonly IMedicalHistoryRepository medicalHistoryRepository;
		private readonly IMapper mapper;

		public GetMedicalHistoryByPatientIdQueryHandler(IMedicalHistoryRepository medicalHistoryRepository, IMapper mapper)
		{
			this.medicalHistoryRepository = medicalHistoryRepository;
			this.mapper = mapper;
		}

		public async Task<Result<List<MedicalHistoryDTO>>> Handle(GetMedicalHistoryByPatientIdQuery request, CancellationToken cancellationToken)
		{
			// Fetch the medical histories from the repository
			var result = await medicalHistoryRepository.GetMedicalHistoryByPatientIdAsync(request.PatientId);

			// Check if the repository operation was successful
			if (!result.IsSuccess)
			{
				return Result<List<MedicalHistoryDTO>>.Failure(result.ErrorMessage);
			}

			// Map the List<MedicalHistory> to List<MedicalHistoryDTO>
			var medicalHistoryDtos = mapper.Map<List<MedicalHistoryDTO>>(result.Data);

			// Return the mapped DTOs as a successful result
			return Result<List<MedicalHistoryDTO>>.Success(medicalHistoryDtos);
		}
	}
}
