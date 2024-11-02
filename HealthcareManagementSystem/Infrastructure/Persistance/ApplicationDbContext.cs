using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasPostgresExtension("uuid-ossp");
			modelBuilder.Entity<User>(entity =>
			{
				entity.ToTable("users");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id)
					  .HasColumnType("uuid")
					  .HasDefaultValueSql("uuid_generate_v4()")
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
			});

			modelBuilder.Entity<Patient>(entity =>
			{
				entity.ToTable("patients");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id)
					  .ValueGeneratedOnAdd();
				entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.DateOfBirth).IsRequired();
				entity.Property(e => e.Gender).IsRequired();
				entity.HasOne(p => p.Doctor)
					  .WithMany(d => d.Patients)
					  .HasForeignKey(p => p.Id)
					  .OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<Doctor>(entity =>
			{
				entity.ToTable("doctors");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id)
					  .ValueGeneratedOnAdd();
				entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Specialization).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
				entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(11);
				entity.HasOne(d => d.User)
					  .WithOne()
					  .HasForeignKey<Doctor>(d => d.Id)
					  .OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Appointment>(entity =>
			{
				entity.ToTable("appointments");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id)
					  .ValueGeneratedOnAdd();
				entity.Property(e => e.AppointmentDate).IsRequired();
				entity.Property(e => e.Reason).HasMaxLength(200);
				entity.Property(e => e.Status)
					.HasConversion<int>()
					.IsRequired();
				entity.HasOne(a => a.Patient)
					  .WithMany(p => p.Appointments)
					  .HasForeignKey(a => a.Id)
					  .OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(a => a.Doctor)
					  .WithMany(d => d.Appointments)
					  .HasForeignKey(a => a.Id)
					  .OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<MedicalHistory>(entity =>
			{
				entity.ToTable("medical_histories");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id)
					  .ValueGeneratedOnAdd();
				entity.Property(e => e.DateRecorded).IsRequired();
				entity.Property(e => e.Diagnosis).HasMaxLength(200);
				entity.Property(e => e.Medications).HasMaxLength(500);
				entity.Property(e => e.Allergies).HasMaxLength(200);
				entity.Property(e => e.Notes).HasMaxLength(1000);
				entity.HasOne(m => m.Patient)
					  .WithMany(p => p.MedicalHistories)
					  .HasForeignKey(m => m.Id)
					  .OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<HealthRiskPrediction>(entity =>
			{
				entity.ToTable("health_risk_predictions");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Id)
					  .ValueGeneratedOnAdd();
				entity.Property(e => e.DateCalculated).IsRequired();
				entity.Property(e => e.RiskFactors).HasMaxLength(500);
				entity.Property(e => e.PredictedRisks).HasMaxLength(500);
				entity.HasOne(h => h.Patient)
					  .WithMany(p => p.HealthRiskPredictions)
					  .HasForeignKey(h => h.Id)
					  .OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}
