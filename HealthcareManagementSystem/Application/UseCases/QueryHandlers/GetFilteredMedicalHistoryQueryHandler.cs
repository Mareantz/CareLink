using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using Gridify;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
    public class GetFilteredMedicalHistoryQueryHandler : IRequestHandler<GetFilteredMedicalHistoryQuery, Result<PagedResult<MedicalHistoryDTO>>>
    {
        private readonly IMedicalHistoryRepository medicalHistoryRepository;
        private readonly IMapper mapper;
        public GetFilteredMedicalHistoryQueryHandler(IMedicalHistoryRepository medicalHistoryRepository, IMapper mapper)
        {
            this.medicalHistoryRepository = medicalHistoryRepository;
            this.mapper = mapper;
        }
        public async Task<Result<PagedResult<MedicalHistoryDTO>>> Handle(GetFilteredMedicalHistoryQuery request, CancellationToken cancellationToken)
        {
            var medicalHistories = await medicalHistoryRepository.PagedResult(
                request.Page,
                request.PageSize,
                request.PatientId,
                request.Medication,
                request.Diagnosis);

            var medicalHistoryDtos = mapper.Map<List<MedicalHistoryDTO>>(medicalHistories.Data);
            var pagedResult = new PagedResult<MedicalHistoryDTO>(medicalHistoryDtos, medicalHistories.TotalCount);
            return Result<PagedResult<MedicalHistoryDTO>>.Success(pagedResult);
        }
    }
}
