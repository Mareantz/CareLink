using Application.DTOs;
using Application.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.QueryHandlers
{
    public class GetPatientsQueryHandler : IRequestHandler<GetPatientsQuery, List<PatientDTO>>
    {
        private readonly IPatientRepository repository;
        private readonly IMapper mapper;

        public GetPatientsQueryHandler(IPatientRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<List<PatientDTO>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await repository.GetPatients();
            return mapper.Map<List<PatientDTO>>(patients);
        }

    }
}
