using Application.UseCases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers
{
	public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, Result>
	{
		private readonly IDoctorRepository repository;
		private readonly IMapper mapper;
		private readonly UpdateDoctorCommandValidator validator;

		public UpdateDoctorCommandHandler(IDoctorRepository repository, IMapper mapper, UpdateDoctorCommandValidator validator)
		{
			this.repository = repository;
			this.mapper = mapper;
			this.validator = validator;
		}

		public async Task<Result> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
		{
			var validationResult = validator.Validate(request);
			if (!validationResult.IsValid)
			{
				var errorsResult = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
				return Result.Failure(string.Join(", ", errorsResult));
			}

			var existingDoctor = await repository.GetDoctorById(request.Id);
			if (existingDoctor == null)
			{
				return Result.Failure($"Doctor with Id {request.Id} not found.");
			}

			var doctor = mapper.Map(request, existingDoctor);
			var updateResult = await repository.UpdateDoctor(doctor);
			if (updateResult.IsSuccess)
			{
				return Result.Success();
			}
			return Result.Failure(updateResult.ErrorMessage);
		}
	}
}
