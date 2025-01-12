using Application.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;
using Application.UseCases.Commands;
using FluentValidation;
namespace Application.UseCases.CommandHandlers
{
    public class UpdateMedicalHistoryCommandHandler : IRequestHandler<UpdateMedicalHistoryCommand, Result>
    {
        private readonly IMedicalHistoryRepository repository;
        private readonly IMapper mapper;
        private readonly UpdateMedicalHistoryCommandValidator validator;
        public UpdateMedicalHistoryCommandHandler(IMedicalHistoryRepository repository, IMapper mapper, UpdateMedicalHistoryCommandValidator validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }
        public async Task<Result> Handle(UpdateMedicalHistoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorsResult = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result.Failure(string.Join(", ", errorsResult));
            }
            var existingMedicalHistory = await repository.GetMedicalHistoryById(request.Id);
            if (existingMedicalHistory == null)
            {
                return Result.Failure($"Medical History with Id {request.Id} not found.");
            }
            var medicalHistory = mapper.Map(request, existingMedicalHistory);
            var updateResult = await repository.UpdateMedicalHistory(medicalHistory);
            if (updateResult.IsSuccess)
            {
                return Result.Success();
            }
            return Result.Failure(updateResult.ErrorMessage);
        }
    }
}
