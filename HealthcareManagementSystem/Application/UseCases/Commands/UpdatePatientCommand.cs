using Domain.Common;
using MediatR;

namespace Application.Commands
{
    public class UpdatePatientCommand : IRequest<Result>
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public required string Address { get; set; }
    }
}
