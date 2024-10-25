using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
		public DbSet<Product> Products { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasPostgresExtension("uuid-ossp");
			modelBuilder.Entity <Product>(

					entity =>
					{
						entity.ToTable("Products");
						entity.HasKey(e => e.Id);
						entity.Property(e => e.Id)
						.HasColumnType("uuid")
						.HasDefaultValueSql("uuid_generate_v4()")
						.ValueGeneratedOnAdd();
						entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
						entity.Property(e => e.Price).IsRequired();
						entity.Property(e => e.Tva).IsRequired();
					}
				);
		}
	}
}
