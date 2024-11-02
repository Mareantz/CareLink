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

		public async Task<int> AddPatient(Patient patient)
		{
			await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();
            return patient.Id;
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
