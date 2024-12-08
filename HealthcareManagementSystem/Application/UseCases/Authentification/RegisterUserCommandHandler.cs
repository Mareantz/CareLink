using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Role = request.Role == UserRole.Admin ? UserRole.Admin : request.Role == UserRole.Doctor ? UserRole.Doctor : UserRole.Patient
        };
        await _userRepository.Register(user, cancellationToken);
        return user.Id;
    }
}

