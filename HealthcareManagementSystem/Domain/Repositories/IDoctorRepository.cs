using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IDoctorRepository
    {
        Task<Result<Guid>> AddDoctor(Doctor doctor);
        Task<IEnumerable<Doctor>> GetDoctors();
        Task<Result> UpdateDoctor(Doctor doctor);
		Task<Doctor?> GetDoctorById(Guid id);
	}
}
