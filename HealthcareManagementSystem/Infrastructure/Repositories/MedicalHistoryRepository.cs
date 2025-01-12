using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using PredictiveHealthcare.Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class MedicalHistoryRepository : IMedicalHistoryRepository
    {
        private readonly ApplicationDbContext context;

        public MedicalHistoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Result<Guid>> AddMedicalHistory(MedicalHistory medicalHistory)
        {
            try
            {
                await context.MedicalHistories.AddAsync(medicalHistory);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(medicalHistory.Id);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
                return Result<Guid>.Failure(errorMessage);
            }
        }



        public async Task<IEnumerable<MedicalHistory>> GetMedicalHistories()
        {
            return await Task.FromResult(context.MedicalHistories.AsEnumerable());
        }

        public async Task<MedicalHistory?> GetMedicalHistoryById(Guid id)
        {
            return await context.MedicalHistories.FindAsync(id);
        }

        public async Task<PagedResult<MedicalHistory>> PagedResult(int page, int pageSize, Guid? patientId, string? Medication, string? Diagnosis)
        {
            var query = context.MedicalHistories.AsQueryable();
            if (patientId != null)
            {
                query = query.Where(x => x.PatientId == patientId);
            }
            if (Medication != null)
            {
                query = query.Where(x => x.Medication.Contains(Medication));
            }
            if (Diagnosis != null)
            {
                query = query.Where(x => x.Diagnosis.Contains(Diagnosis));
            }
            var totalItems = query.CountAsync();
            var data = await query.
                OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize).
                Take(pageSize).
                ToListAsync();
            return new PagedResult<MedicalHistory>(data, totalItems);
        }

        public async Task<Result> UpdateMedicalHistory(MedicalHistory medicalHistory)
        {
            try
            {
                context.Entry(medicalHistory).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
                return Result.Failure(errorMessage);
            }
        }
    }
}