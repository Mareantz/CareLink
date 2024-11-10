using Application.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.CommandHandlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UpdateUserCommandHandler(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            //UpdatePatientCommandValidator validationRules = new UpdatePatientCommandValidator();
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

            var user = mapper.Map<User>(request);
            return repository.UpdateUser(user);
        }
    }
}
