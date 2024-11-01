using Domain.Entities;

namespace Domain.Repositories
{
	public interface IPatientRepository
	{
		Task<int> AddPatient(Patient patient);
		Task<IEnumerable<Patient>> GetPatients();
		Task UpdatePatient(Patient patient);
	}
}
