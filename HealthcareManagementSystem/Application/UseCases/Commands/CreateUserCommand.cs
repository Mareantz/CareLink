using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Commands
{
    public class CreateUserCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public UserRole Role { get; set; } = 0;
    }
}
