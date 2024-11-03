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

		public async Task<Result<int>> AddPatient(Patient patient)
		{
			try
			{
				await context.Patients.AddAsync(patient);
				await context.SaveChangesAsync();
				return Result<int>.Success(patient.Id);
			}
			catch (Exception ex)
			{
				return Result<int>.Failure(ex.InnerException!.ToString());
			}
        }

		public async Task<IEnumerable<Patient>> GetPatients()
		{
			return await context.Patients.ToListAsync();
        }

		public async Task UpdatePatient(Patient patient)
		{
			context.Entry(patient).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
	}
}
