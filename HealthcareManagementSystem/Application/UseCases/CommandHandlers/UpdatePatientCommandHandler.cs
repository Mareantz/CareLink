using Application.Commands;
using AutoMapper;
using Domain.Repositories;
using FluentValidation;
using MediatR;

public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand>
{
    private readonly IPatientRepository repository;
    private readonly IMapper mapper;
    private readonly IValidator<UpdatePatientCommand> validator;

    public UpdatePatientCommandHandler(IPatientRepository repository, IMapper mapper, IValidator<UpdatePatientCommand> validator)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.validator = validator;
    }

    public async Task Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errorsResult = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException(string.Join(", ", errorsResult));
        }

        var existingPatient = await repository.GetPatientById(request.Id);
        if (existingPatient == null)
        {
            throw new KeyNotFoundException($"Patient with Id {request.Id} not found.");
        }

        var patient = mapper.Map(request, existingPatient);
        await repository.UpdatePatient(patient);
    }
}
