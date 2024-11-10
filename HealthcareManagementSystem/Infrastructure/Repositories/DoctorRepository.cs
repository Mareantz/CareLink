using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using PredictiveHealthcare.Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext context;

        public DoctorRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<Guid>> AddDoctor(Doctor doctor)
        {
            try
            {
                await context.Doctors.AddAsync(doctor);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(doctor.UserId);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
                return Result<Guid>.Failure(errorMessage);
            }

        }

        public async Task<IEnumerable<Doctor>> GetDoctors()
        {
            return await context.Doctors.ToListAsync();
        }

        public async Task<Result> UpdateDoctor(Doctor doctor)
        {
            try
            {
                context.Entry(doctor).State = EntityState.Modified;
				await context.SaveChangesAsync();
                return Result.Success();
			}
            catch (Exception ex)
			{
				var errorMessage = ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
				return Result.Failure(errorMessage);
			}
		}

        public async Task<Doctor?> GetDoctorById(Guid id)
        {
            return await context.Doctors.FindAsync(id);
		}
    }
}
