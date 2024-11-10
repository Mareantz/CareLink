using Application.Commands;
using Application.UseCases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.CommandHandlers
{
    public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand>
    {
        private readonly IDoctorRepository repository;
        private readonly IMapper mapper;

        public UpdateDoctorCommandHandler(IDoctorRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Task Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            //UpdateDoctorCommandValidator validationRules = new UpdateDoctorCommandValidator();
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

            var doctor = mapper.Map<Doctor>(request);
            return repository.UpdateDoctor(doctor);
        }
    }
}
