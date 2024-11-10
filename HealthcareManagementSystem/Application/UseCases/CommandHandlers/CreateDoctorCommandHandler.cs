using Application.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Common;
using MediatR;
using Application.UseCases.Commands;

namespace Application.UseCases.CommandHandlers
{
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Result<Guid>>
    {
        private readonly IDoctorRepository repository;
        private readonly IMapper mapper;

        public CreateDoctorCommandHandler(IDoctorRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = mapper.Map<Doctor>(request);
            doctor.UserId = new Guid("33333333-3333-3333-3333-333333333333");//very hardcoded for testing
            var result = await repository.AddDoctor(doctor);
            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }

}
