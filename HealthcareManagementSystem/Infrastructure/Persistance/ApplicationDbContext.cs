using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PredictiveHealthcare.Infrastructure.Persistence
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
		public DbSet<MedicalHistory> MedicalHistories { get; set; }
		public DbSet<HealthRiskPrediction> HealthRiskPredictions { get; set; }
		private const string UuidGenerationFunction = "uuid_generate_v4()";

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.HasPostgresExtension("uuid-ossp");

			var stringListComparer = new ValueComparer<List<string>>(
				(c1, c2) =>
					c1 != null && c2 != null ? c1.SequenceEqual(c2) : c1 == c2,
				c =>
					c != null ? c.Aggregate(0, (a, v) => HashCode.Combine(a, v != null ? v.GetHashCode() : 0)) : 0,
				c =>
					c != null ? new List<string>(c) : new List<string>()
			);
			modelBuilder.Entity<User>(entity =>
			{
				entity.ToTable("users");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id)
					  .HasColumnType("uuid")
					  .HasDefaultValueSql(UuidGenerationFunction)
					  .ValueGeneratedOnAdd();
				entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
				entity.HasIndex(e => e.Username).IsUnique();
				entity.Property(e => e.PasswordHash).IsRequired();
				entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
				entity.HasIndex(e => e.Email).IsUnique();
				entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(11);
				entity.Property(e => e.Role)
					  .HasConversion<int>()
					  .IsRequired();
				entity.HasData(
					new User
					{
						Id = new Guid("11111111-1111-1111-1111-111111111111"),
						Username = "testuser1",
						PasswordHash = "hashed_password1",
						Email = "testuser1@example.com",
						PhoneNumber = "0700000001",
						Role = UserRole.Patient
					},
					new User
					{
						Id = new Guid("22222222-2222-2222-2222-222222222222"),
						Username = "testuser2",
						PasswordHash = "hashed_password2",
						Email = "testuser2@example.com",
						PhoneNumber = "0700000002",
						Role = UserRole.Patient
					},
					new User
					{
						Id = new Guid("33333333-3333-3333-3333-333333333333"),
						Username = "testuser3",
						PasswordHash = "hashed_password3",
						Email = "testuser3@example.com",
						PhoneNumber = "0700000003",
						Role = UserRole.Doctor
					}
				);
			});

			modelBuilder.Entity<Patient>(entity =>
			{
				entity.ToTable("patients");
				entity.HasKey(e => e.UserId);
				entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.DateOfBirth).IsRequired();
				entity.Property(e => e.Gender).IsRequired();
				entity.Property(e => e.Allergies)
					  .HasConversion
					  (
						  v => string.Join(',', v),
						  v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
					  )
					  .Metadata
					  .SetValueComparer(stringListComparer);
				entity.HasMany(p => p.Doctors)
					  .WithMany(d => d.Patients)
					  .UsingEntity(j => j.ToTable("PatientsToDoctorsTable"));
				entity.HasOne(p => p.User)
					  .WithOne()
					  .HasForeignKey<Patient>(p => p.UserId)
					  .OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Doctor>(entity =>
			{
				entity.ToTable("doctors");
				entity.HasKey(e => e.UserId);
				entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
				entity.HasMany(d => d.Patients)
					  .WithMany(p => p.Doctors);
				entity.HasOne(d => d.User)
					  .WithOne()
					  .HasForeignKey<Doctor>(d => d.UserId)
					  .OnDelete(DeleteBehavior.Cascade);
			});


			modelBuilder.Entity<Appointment>(entity =>
			{
				entity.ToTable("appointments");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id)
					  .HasColumnType("uuid")
					  .HasDefaultValueSql(UuidGenerationFunction)
					  .ValueGeneratedOnAdd();
				entity.Property(e => e.AppointmentDate).IsRequired();
				entity.Property(e => e.Reason).HasMaxLength(200);
				entity.Property(e => e.Status)
					  .HasConversion<int>()
					  .IsRequired();
				entity.HasOne(a => a.Patient)
					  .WithMany(p => p.Appointments)
					  .HasForeignKey(a => a.PatientId)
					  .OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(a => a.Doctor)
					  .WithMany(d => d.Appointments)
					  .HasForeignKey(a => a.DoctorId)
					  .OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<MedicalHistory>(entity =>
			{
				entity.ToTable("medical_histories");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id)
					  .HasColumnType("uuid")
					  .HasDefaultValueSql(UuidGenerationFunction)
					  .ValueGeneratedOnAdd();
				entity.Property(e => e.DateRecorded).IsRequired();
				entity.Property(e => e.Diagnosis).HasMaxLength(200);
				entity.Property(e => e.Notes).HasMaxLength(1000);
				entity.HasOne(m => m.Patient)
					  .WithMany(p => p.MedicalHistories)
					  .HasForeignKey(m => m.PatientId)
					  .OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<HealthRiskPrediction>(entity =>
			{
				entity.ToTable("health_risk_predictions");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id)
					  .HasColumnType("uuid")
					  .HasDefaultValueSql(UuidGenerationFunction)
					  .ValueGeneratedOnAdd();
				entity.Property(e => e.LastUpdate);
				entity.Property(e => e.RiskFactors)
					  .HasConversion
					  (
						 v => string.Join(',', v),
						 v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
					  )
					  .Metadata
					  .SetValueComparer(stringListComparer);
				entity.Property(e => e.PredictedRisks)
					  .HasConversion
					  (
						 v => string.Join(',', v),
						 v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
					  )
					  .Metadata
					  .SetValueComparer(stringListComparer);
				entity.HasOne(h => h.Patient)
					  .WithMany(p => p.HealthRiskPredictions)
					  .HasForeignKey(h => h.PatientId)
					  .OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}
