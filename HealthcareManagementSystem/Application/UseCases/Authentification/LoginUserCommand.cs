using Domain.Common;
using MediatR;
namespace Application.UseCases.Authentification
{
	public class LoginUserCommand : IRequest<Result<string>>
	{
		public required string Username { get; set; }
		public required string Password { get; set; }
	}
}

