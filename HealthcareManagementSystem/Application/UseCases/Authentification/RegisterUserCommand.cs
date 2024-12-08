using MediatR;
public class RegisterUserCommand : IRequest<Guid>
    {
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

}

