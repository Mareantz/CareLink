using Application.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.CommandHandlers
{
    internal class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand>
    {
        private readonly IPatientRepository repository;
        private readonly IMapper mapper;

        public UpdatePatientCommandHandler(IPatientRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Task Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = mapper.Map<Patient>(request);
            return repository.UpdatePatient(patient);
        }
    }
}
