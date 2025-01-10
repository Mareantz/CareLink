using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPatientRepository
    {
        Task<Result<Guid>> AddPatient(Patient patient);
        Task<IEnumerable<Patient>> GetPatients();
        Task<Result> UpdatePatient(Patient patient);
        Task<Patient?> GetPatientById(Guid id);
		Task<PagedResult<Patient>> GetFilteredPatientsAsync(
			int page,
			int pageSize,
			string? firstName,
			string? lastName,
			string? gender,
			DateOnly? dateOfBirth);
	}
}
