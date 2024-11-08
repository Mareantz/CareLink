using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IPatientRepository
	{
		Task<Result<Guid>> AddPatient(Patient patient);
		Task<IEnumerable<Patient>> GetPatients();
		Task UpdatePatient(Patient patient);
	}
}
