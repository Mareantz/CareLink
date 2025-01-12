using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Authentification
{
	public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<string>>
	{
		private readonly IUserRepository _userRepository;
		public LoginUserCommandHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}
		public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
		{
			var loginResult = await _userRepository.Login(request.Username, request.Password);

			if (!loginResult.IsSuccess)
			{
				return Result<string>.Failure(loginResult.ErrorMessage);
			}

			return Result<string>.Success(loginResult.Data);
		}
	}
}
