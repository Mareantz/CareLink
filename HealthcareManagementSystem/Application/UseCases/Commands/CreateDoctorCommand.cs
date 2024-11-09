using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands
{
    public class CreateDoctorCommand : IRequest<Result<Guid>>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Gender { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
    }
}
