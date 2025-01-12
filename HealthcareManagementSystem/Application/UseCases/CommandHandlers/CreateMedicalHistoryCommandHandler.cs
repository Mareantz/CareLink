using Application.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Common;
using MediatR;

namespace Application.CommandHandlers
{
    public class CreateMedicalHistoryCommandHandler : IRequestHandler<CreateMedicalHistoryCommand, Result<Guid>>
    {
        private readonly IMedicalHistoryRepository repository;
        private readonly IMapper mapper;
        public CreateMedicalHistoryCommandHandler(IMedicalHistoryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(CreateMedicalHistoryCommand request, CancellationToken cancellationToken)
        {
            var medicalHistory = mapper.Map<MedicalHistory>(request);
            var result = await repository.AddMedicalHistory(medicalHistory);
            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
    

    }

