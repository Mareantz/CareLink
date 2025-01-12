using Application.DTOs;
using Application.Queries;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
    public class GetMedicalHistoriesQueryHandler : IRequestHandler<GetMedicalHistoriesQuery, List<MedicalHistoryDTO>>
    {
        private readonly IMedicalHistoryRepository repository;
        private readonly IMapper mapper;

        public GetMedicalHistoriesQueryHandler(IMedicalHistoryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<List<MedicalHistoryDTO>> Handle(GetMedicalHistoriesQuery request, CancellationToken cancellationToken)
        {
            var medicalHistories = await repository.GetMedicalHistories();
            return mapper.Map<List<MedicalHistoryDTO>>(medicalHistories);
        }
    }
}
