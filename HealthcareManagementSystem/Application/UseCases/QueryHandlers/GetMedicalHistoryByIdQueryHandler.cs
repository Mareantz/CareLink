using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;



namespace Application.UseCases.QueryHandlers
{
    public class GetMedicalHistoryByIdQueryHandler : IRequestHandler<GetMedicalHistoryByIdQuery, MedicalHistoryDTO>
    {
        private readonly IMedicalHistoryRepository repository;
        private readonly IMapper mapper;
        public GetMedicalHistoryByIdQueryHandler(IMedicalHistoryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<MedicalHistoryDTO> Handle(GetMedicalHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var medicalHistory = await repository.GetMedicalHistoryById(request.Id);
            return mapper.Map<MedicalHistoryDTO>(medicalHistory);
        }
    }
}
