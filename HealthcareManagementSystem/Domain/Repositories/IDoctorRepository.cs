using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IDoctorRepository
    {
        Task<Result<Guid>> AddDoctor(Doctor patient);
        Task<IEnumerable<Doctor>> GetDoctors();
        Task UpdateDoctor(Doctor patient);
    }
}
