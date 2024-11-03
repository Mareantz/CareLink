using Application.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.CommandHandlers
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, int>
    {
        private readonly IPatientRepository repository;
        private readonly IMapper mapper;

        public CreatePatientCommandHandler(IPatientRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<int> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            //CreatePatientCommandValidator validationRules= new CreatePatientCommandValidator();
            //var validator=validationRules.Validate(request);

            //if(!validator.IsValid)
            //{
            //    var errorsResult =new List<string>();
            //    foreach (var error in validator.Errors)
            //    {
            //        errorsResult.Add(error.ErrorMessage);
            //    }

            //    throw new ValidationException(errorsResult.ToString());
            //}    

            var patient = mapper.Map<Patient>(request);
			return await repository.AddPatient(patient);
        }
   
    }
}
