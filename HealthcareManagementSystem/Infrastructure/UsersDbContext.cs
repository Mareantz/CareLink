using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure
{
	public class UsersDbContext : DbContext
	{
		public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
		{
		}
		public DbSet<User> Users { get; set; }
	}
}
