using Application.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Common;
using FluentValidation;
using MediatR;

namespace Application.CommandHandlers
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Result<int>>
    {
        private readonly IPatientRepository repository;
        private readonly IMapper mapper;

        public CreatePatientCommandHandler(IPatientRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
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
			var result = await repository.AddPatient(patient);
            if(result.IsSuccess)
            {
                return Result<int>.Success(result.Data);
			}
            return Result<int>.Failure(result.ErrorMessage);
		}
   
    }
}
