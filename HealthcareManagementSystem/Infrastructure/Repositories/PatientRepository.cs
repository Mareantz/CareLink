using Domain.Entities;
using Domain.Repositories;
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

		public Task<int> AddPatient(Patient patient)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Patient>> GetPatients()
		{
			throw new NotImplementedException();
		}

		public Task UpdatePatient(Patient patient)
		{
			throw new NotImplementedException();
		}
	}
}
