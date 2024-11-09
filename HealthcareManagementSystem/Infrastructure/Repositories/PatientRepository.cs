using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using PredictiveHealthcare.Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
	public class PatientRepository : IPatientRepository
	{
		private readonly ApplicationDbContext context;

		public PatientRepository(ApplicationDbContext context)
		{
            this.context = context;
        }

		public async Task<Result<Guid>> AddPatient(Patient patient)
		{
			try
			{
				await context.Patients.AddAsync(patient);
				await context.SaveChangesAsync();
				return Result<Guid>.Success(patient.UserId);
			}
			catch (Exception ex)
			{
				var errorMessage = ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
				return Result<Guid>.Failure(errorMessage);
			}

		}

		public async Task<IEnumerable<Patient>> GetPatients()
		{
			return await context.Patients.ToListAsync();
        }
        public async Task<Patient?> GetPatientById(Guid id)
        {
            return await context.Patients.FindAsync(id);
        }
        public  Task UpdatePatient(Patient patient)
		{
			try
			{
				
				context.Entry(patient).State = EntityState.Modified;
				return context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				var errorMessage = ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
				return Task.FromException(new Exception(errorMessage));
			}
        }
	}
}
