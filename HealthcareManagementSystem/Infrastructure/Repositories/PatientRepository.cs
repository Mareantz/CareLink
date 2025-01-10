using Domain.Common;
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

        public async Task<Result<Guid>> AddPatient(Patient patient)
        {
            try
            {
                await context.Patients.AddAsync(patient);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(patient.UserId);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
                return Result<Guid>.Failure(errorMessage);
            }
        }

        public async Task<IEnumerable<Patient>> GetPatients()
        {
            return await context.Patients.ToListAsync();
        }

		public IQueryable<Patient> GetFilteredPatients()
		{
			return context.Patients;
		}

		public async Task<Patient?> GetPatientById(Guid id)
        {
            return await context.Patients.FindAsync(id);
        }

        public async Task<Result> UpdatePatient(Patient patient)
        {
            try
            {
                context.Entry(patient).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
                return Result.Failure(errorMessage);
            }
        }

		public async Task<PagedResult<Patient>> GetFilteredPatientsAsync(
			int page,
			int pageSize,
			string? firstName,
			string? lastName,
			string? gender,
			DateOnly? dateOfBirth)
		{
			var query = context.Patients.AsQueryable();

			if (!string.IsNullOrWhiteSpace(firstName))
			{
				query = query.Where(p => p.FirstName.Contains(firstName));
			}

			if (!string.IsNullOrWhiteSpace(lastName))
			{
				query = query.Where(p => p.LastName.Contains(lastName));
			}

			if (!string.IsNullOrWhiteSpace(gender))
			{
				query = query.Where(p => p.Gender == gender);
			}

			if (dateOfBirth.HasValue)
			{
				query = query.Where(p => p.DateOfBirth == dateOfBirth.Value);
			}

			var totalCount = await query.CountAsync();

			var data = await query
				.OrderBy(p => p.LastName)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return new PagedResult<Patient>(data, totalCount);
		}
	}
}
