using Application.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Result>
    {
        private readonly IPatientRepository repository;
        private readonly IMapper mapper;
        private readonly UpdatePatientCommandValidator validator;

        public UpdatePatientCommandHandler(IPatientRepository repository, IMapper mapper, UpdatePatientCommandValidator validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<Result> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorsResult = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result.Failure(string.Join(", ", errorsResult));
            }

            var existingPatient = await repository.GetPatientById(request.Id);
            if (existingPatient == null)
            {
                return Result.Failure($"Patient with Id {request.Id} not found.");
            }

            var patient = mapper.Map(request, existingPatient);
            var updateResult = await repository.UpdatePatient(patient);
            if (updateResult.IsSuccess)
            {
                return Result.Success();
            }
            return Result.Failure(updateResult.ErrorMessage);
        }
    }
}
