using Application.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Common;
using MediatR;

namespace Application.CommandHandlers
{
	public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Result<Guid>>
    {
        private readonly IPatientRepository repository;
        private readonly IMapper mapper;

        public CreatePatientCommandHandler(IPatientRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            DateOnly dateOfBirth = ParseDateOfBirth(request.DateOfBirth);
			var patient = mapper.Map<Patient>(request);
			patient.DateOfBirth = dateOfBirth;
            patient.UserId = new Guid("11111111-1111-1111-1111-111111111111");//very hardcoded for testing
			var result = await repository.AddPatient(patient);
            if(result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
			}
            return Result<Guid>.Failure(result.ErrorMessage);
		}

		private DateOnly ParseDateOfBirth(string dateOfBirth)
		{
			return DateOnly.ParseExact(dateOfBirth, "dd-MM-yyyy");
		}

	}
}
