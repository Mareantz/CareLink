using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using PredictiveHealthcare.Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
	public class AppointmentRepository : IAppointmentRepository
	{
		private readonly ApplicationDbContext context;

		public AppointmentRepository(ApplicationDbContext context)
		{
			this.context = context;
		}
		public async Task<Result<Guid>> AddAppointment(Appointment appointment)
		{
			try
			{
				await context.Appointments.AddAsync(appointment);
				await context.SaveChangesAsync();
				return Result<Guid>.Success(appointment.Id);
			}
			catch (Exception ex)
			{
				var errorMessage = ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
				return Result<Guid>.Failure(errorMessage);
			}
		}

		public Task<Appointment?> GetAppointmentById(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Appointment>> GetAppointments()
		{
			throw new NotImplementedException();
		}

		public Task<Result> UpdateAppointment(Appointment appointment)
		{
			throw new NotImplementedException();
		}
	}
}
