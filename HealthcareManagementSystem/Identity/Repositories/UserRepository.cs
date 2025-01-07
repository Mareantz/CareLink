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

		public async Task<string> Login(User user)
		{
			var existingUser = await usersDbContext.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
			if (existingUser == null)
			{
				throw new UnauthorizedAccessException("Invalid credentials");
			}
			if (!BCrypt.Net.BCrypt.Verify(user.PasswordHash, existingUser.PasswordHash))
			{
				throw new UnauthorizedAccessException("Invalid credentials");
			}
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
			new Claim(ClaimTypes.Name, existingUser.Id.ToString()),
			new Claim(ClaimTypes.Role, existingUser.Role.ToString())
		}),
				Expires = DateTime.UtcNow.AddHours(3),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
		public async Task<Guid> Register(User user, CancellationToken cancellationToken)
        {
            usersDbContext.Users.Add(user);
            await usersDbContext.SaveChangesAsync(cancellationToken);
            return user.Id;
        }

        public Task UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
