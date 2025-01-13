using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using PredictiveHealthcare.Infrastructure.Persistence;

namespace Identity.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext usersDbContext;
		private readonly IConfiguration configuration;

		public UserRepository(ApplicationDbContext usersDbContext, IConfiguration configuration)
		{
			this.usersDbContext = usersDbContext;
			this.configuration = configuration;
		}

		public Task<Result<Guid>> AddUser(User user)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<User>> GetUsers()
		{
			return await usersDbContext.Users.ToListAsync();
		}

		public async Task<Result<string>> Login(string username, string password)
		{
			var existingUser = await usersDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
			if (existingUser == null)
			{
				return Result<string>.Failure("Invalid credentials.");
			}
			bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, existingUser.PasswordHash);
			if (!isPasswordValid)
			{
				return Result<string>.Failure("Invalid credentials.");
			}
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
			new Claim("user_id", existingUser.Id.ToString()),
			new Claim(ClaimTypes.Role, existingUser.Role.ToString())
		}),
				Expires = DateTime.UtcNow.AddHours(3),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);
			return Result<string>.Success(tokenString);
		}
		public async Task<Result<Guid>> Register(User user, CancellationToken cancellationToken)
		{
			try
			{
				usersDbContext.Users.Add(user);
				await usersDbContext.SaveChangesAsync(cancellationToken);
				return Result<Guid>.Success(user.Id);
			}
			catch (Exception ex)
			{
				return Result<Guid>.Failure($"Registration failed: {ex.Message}");
			}
		}

		public Task UpdateUser(User user)
		{
			throw new NotImplementedException();
		}
	}
}
