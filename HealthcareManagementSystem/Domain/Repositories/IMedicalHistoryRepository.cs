using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMedicalHistoryRepository
    {
        Task<Result<Guid>> AddMedicalHistory(MedicalHistory medicalHistory);
        Task<IEnumerable<MedicalHistory>> GetMedicalHistories();
        Task<Result> UpdateMedicalHistory(MedicalHistory medicalHistory);
        Task<MedicalHistory?> GetMedicalHistoryById(Guid id);
        Task<PagedResult<MedicalHistory>> PagedResult(
            int page,
            int pageSize,
            Guid? patientId,
            string? Medication,
            string? Diagnosis);
    }
}
