using Application.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.CommandHandlers
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand>
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
            //UpdatePatientCommandValidator validationRules = new UpdatePatientCommandValidator();
            //var validator = validationRules.Validate(request);

            //if (!validator.IsValid)
            //{
            //    var errorsResult = new List<string>();
            //    foreach (var error in validator.Errors)
            //    {
            //        errorsResult.Add(error.ErrorMessage);
            //    }

            //    throw new ValidationException(errorsResult.ToString());
            //}

            var patient = mapper.Map<Patient>(request);
            return repository.UpdatePatient(patient);
        }
    }
}
