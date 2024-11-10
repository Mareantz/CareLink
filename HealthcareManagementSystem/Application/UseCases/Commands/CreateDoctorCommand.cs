using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands
{
    public class CreateDoctorCommand : IRequest<Result<Guid>>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Specialization { get; set; }
        public required string Bio { get; set; }
    }
}
