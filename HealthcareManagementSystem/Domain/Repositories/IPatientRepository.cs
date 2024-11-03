using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IPatientRepository
	{
		Task<Result<int>> AddPatient(Patient patient);
		Task<IEnumerable<Patient>> GetPatients();
		Task UpdatePatient(Patient patient);
	}
}
